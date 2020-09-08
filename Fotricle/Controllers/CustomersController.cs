using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using Fotricle.Models;
using Fotricle.Security;
using Microsoft.Ajax.Utilities;

namespace Fotricle.Controllers
{
    public class CustomersController : ApiController
    {
        private Model1 db = new Model1();


        //註冊
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("customer/register")]
        public IHttpActionResult PostRegister(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //檢查Email是否重複
            var checkEmail = db.Customers.FirstOrDefault(c => c.Email == customer.Email);

            if (checkEmail != null)
            {
                return Ok(new
                {
                    result = false,
                    message = "此Email已經註冊"
                });
            }

            customer.PasswordSalt = Utility.CreateSalt();//建立鹽
            customer.Password = Utility.GenerateHashWithSalt(customer.Password, customer.PasswordSalt);//密碼+鹽後加密

            db.Customers.Add(customer);
            db.SaveChanges();
            return Ok(new
            {
                result = true,
                message = "註冊成功"
            });
        }


        //登入
        [System.Web.Http.Route("customer/login")]
        public HttpResponseMessage PostLogin(ViewLogin viewLogin)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    result = false,
                    message = "登入失敗!"
                });
            }

            Customer customer = ValidateUser(viewLogin.Email, viewLogin.Password);//檢查會員登入密碼

            if (customer == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new
                {
                    result = false,
                    message = "帳號或密碼錯誤!"
                });
            }

            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            string jwtToken = jwtAuthUtil.GenerateToken(customer.Id);
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                result = true,
                message = "登入成功",
                id = customer.Id,
                token = jwtToken


            });
        }


        //登入驗證
        private Customer ValidateUser(string userName, string password)
        {
            Customer customer = db.Customers.SingleOrDefault(o => o.Email == userName);


            if (customer == null)
            {

                return null;
            }
            string saltPassword = Utility.GenerateHashWithSalt(password, customer.PasswordSalt);
            return saltPassword == customer.Password ? customer : null;
        }


        /// 顧客更新資料
        [System.Web.Http.HttpPatch]
        [JwtAuthFilter]
        [System.Web.Http.Route("customer/Edit")]
        public IHttpActionResult EditCustomer(string id, [Bind(Include = "Id,UserName,CusPhone,Gender,Age")] ViewCustomer viewCustomer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Customer customer = new Customer();
            int id_temp = Convert.ToInt32(id);
            customer = db.Customers.Where(c => c.Id == id_temp).FirstOrDefault();
            customer.UserName = viewCustomer.UserName;
            customer.CusPhone = viewCustomer.CusPhone;
            customer.Gender = viewCustomer.Gender;
            customer.Age = viewCustomer.Age;


            db.Entry(customer).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(new
            {
                result = true,
                message = "更新成功"
            });
        }


        //上傳顧客圖片
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("customer/upload")]
        [JwtAuthFilter]
        public HttpResponseMessage PostUploadImage()
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int id = Convert.ToInt32(jwtAuthUtil.GetId(token));
            Customer customer = db.Customers.Find(id);

            try
            {
                var file = HttpContext.Current.Request.Files.Count > 0
                    ? HttpContext.Current.Request.Files[0] : null;

                if (file != null && file.ContentLength > 0)
                {
                    //新的檔案名稱
                    string fileName = Utility.UploadImage(file);


                    //產生圖片連結
                    UriBuilder uriBuilder = new UriBuilder(HttpContext.Current.Request.Url)
                    {
                        Path = $"/Upload/customer/{fileName}"
                    };

                    Uri imageUrl = uriBuilder.Uri;
                    customer.CusPhone = imageUrl.ToString();

                    db.Entry(customer).State = EntityState.Modified;
                    db.SaveChanges();


                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        success = true,
                        message = "已上傳個人圖片",
                        imageUrl

                    });

                }


                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    result = false,
                    message = "請選擇上傳圖片!"
                });

            }

            catch
            {
                throw;
            }
        }


        //Get顧客單一資料
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/customer/{id}")]
        [JwtAuthFilter]
        public IHttpActionResult GetCustomer(int id)
        {

            Customer customer = db.Customers.Find(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                result = true,
                member = new
                {
                    customer.Id,
                    customer.UserName,
                    customer.CusPhone,
                    customer.Email,
                    customer.Gender,
                    customer.Age,
                }
            });
        }


        //Get顧客全部資料
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("member/All")]
        public IHttpActionResult GetGustomers()
        {
            var customer = db.Customers.Select(c => new
            {
                c.Id,
                c.Email,
                c.UserName,
                c.CusPhone,
                c.CusPhoto,

            });
            return Ok(new { success = true, customer });
        }



        //// GET: api/Customers
        //    public IQueryable<Customer> GetCustomers()
        //{
        //    return db.Customers;
        //}



        // PUT: api/Customers/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutCustomer(int id, Customer customer)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != customer.Id)
        //    {
        //        return BadRequest();
        //    }

        //    customer.PasswordSalt = Utility.CreateSalt();
        //    customer.Password = Utility.GenerateHashWithSalt(customer.Password, customer.PasswordSalt);
        //    db.Entry(customer).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CustomerExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}


        // DELETE: api/Customers/5
        [ResponseType(typeof(Customer))]
        public IHttpActionResult DeleteCustomer(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
            db.SaveChanges();

            return Ok(customer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExists(int id)
        {
            return db.Customers.Count(e => e.Id == id) > 0;
        }
    }
}