using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using Fotricle.Models;
using Fotricle.Security;


namespace Fotricle.Controllers
{
    public class BrandsController : ApiController
    {
        private Model1 db = new Model1();

        //註冊
        [System.Web.Http.Route("Brand/register")]
        public IHttpActionResult PostRegister(Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //檢查Email是否重複
            var checkEmail = db.Brands.FirstOrDefault(b => b.Email == brand.Email);

            if (checkEmail != null)
            {
                return Ok(new
                {
                    result = false,
                    message = "此Email已經註冊"
                });
            }

            brand.PasswordSalt = Utility.CreateSalt();//建立鹽
            brand.Password = Utility.GenerateHashWithSalt(brand.Password, brand.PasswordSalt);//密碼+鹽後加密

            db.Brands.Add(brand);
            db.SaveChanges();
            return Ok(new
            {
                result = true,
                message = "註冊成功"
            });
        }

        //登入
        [System.Web.Http.Route("Brand/login")]
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

            Brand brand = ValidateUser(viewLogin.Email, viewLogin.Password);//檢查會員登入密碼
            if (brand == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new
                {
                    result = false,
                    message = "帳號或密碼錯誤!"
                });
            }
            else if (brand.Verification != Verification.是)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new
                {
                    result = false,
                    message = "帳號尚未驗證開通!"
                });
            }


            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            string jwtToken = jwtAuthUtil.GenerateToken(brand.Id,"1");
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                result = true,
                message = "登入成功",
                id = brand.Id,
                token = jwtToken

            });
        }

        //登入驗證
        private Brand ValidateUser(string Email, string password)
        {
            Brand brand = db.Brands.SingleOrDefault(o => o.Email == Email);

            if (brand == null)
            {

                return null;
            }
            string saltPassword = Utility.GenerateHashWithSalt(password, brand.PasswordSalt);
            return saltPassword == brand.Password ? brand : null;
        }



        /// 登入品牌後新增詳細資料
        [System.Web.Http.HttpPatch]
        [JwtAuthFilter]
        [System.Web.Http.Route("Brand/Edit")]
        public IHttpActionResult EditBrands(string id, [Bind(Include = "Id,BrandName,BrandStory,PhoneNumber,Sort,LinePay,CarImage,LogoPhoto,OrCode")] ViewBrand viewBrand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Brand brand = new Brand();
            int id_temp = Convert.ToInt32(id);
            brand = db.Brands.Where(x => x.Id == id_temp).FirstOrDefault();
            brand.BrandName = viewBrand.BrandName;
            brand.BrandStory = viewBrand.BrandStory;
            brand.PhoneNumber = viewBrand.PhoneNumber;

            brand.Sort = viewBrand.Sort;
            brand.LinePay = viewBrand.LinePay;
            brand.CarImage = viewBrand.CarImage;
            brand.LogoPhoto = viewBrand.LogoPhoto;
            brand.QrCode = viewBrand.QrCode;


            db.Entry(brand).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(new
            {
                brand,
                sort=brand.Sort.ToString(),
                result = true,
                message = "新增修改成功"
            });
        }



        ///審核業主帳號是否通過
        [System.Web.Http.HttpPatch]
        [System.Web.Http.Route("Brand/checkpass")]
        public IHttpActionResult PatchBrands(string id, [Bind(Include = "Id,Verification")] ViewBrand viewBrand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Brand brand = new Brand();
            int id_temp = Convert.ToInt32(id);
            brand = db.Brands.Where(x => x.Id == id_temp).FirstOrDefault();
            brand.Verification = viewBrand.Verification;

            db.Entry(brand).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(new
            {
                result = true,
                message = "修改成功"
            });
        }



        //Get品牌單一資料
        [System.Web.Http.HttpGet]
        [JwtAuthFilter]
        [System.Web.Http.Route("Brand/Detail")]
        //public IHttpActionResult BrandDetail(string id,
        //    [Bind(Include = "Id,BrandName,BrandStory,PhoneNumber,Sort,LinePay,CarImage,QrCode,LogoPhoto")]
        //    ViewBrand viewBrand)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    Brand brand = db.Brands.Find(id);
        //    brand.BrandName = viewBrand.BrandName;
        //    brand.BrandStory =viewBrand.BrandStory;
        //    brand.PhoneNumber = viewBrand.PhoneNumber;
        //    brand.Sort = viewBrand.Sort;
        //    brand.LinePay = viewBrand.LinePay;
        //    brand.CarImage = viewBrand.CarImage;
        //    brand.LogoPhoto = viewBrand.LogoPhoto;
        //    brand.QrCode = viewBrand.QrCode;
        //    if (brand== null)
        //    {
        //        return Ok(new
        //        {
        //            result = false,
        //            ModelState,
        //            message = "載入失敗"
        //        });

        //    }

        //    return Ok(viewBrand);

        //}
        public IHttpActionResult BrandDetail(int? Id)
        {
            if (Id == null)
            {
                return Ok(new
                {
                    result = false,
                    ModelState,
                    message = "載入失敗"
                });
            }

            Brand brand = db.Brands.Find(Id);

            return Ok(new
            {
                brand,
                sort = brand.Sort.ToString(),
       
            });
        
        }


        //Get餐車品牌全部
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("Brand/All")]
        // GET: api/Brands
        public IQueryable<Brand> GetBrands()
        {
            return db.Brands;
        }

        //Get餐車品牌單一名稱
        [System.Web.Http.HttpGet]
        [JwtAuthFilter]
        [System.Web.Http.Route("Brand/GetBrand")]
        public IHttpActionResult GetBrands(int id)
        {
            Brand brand = db.Brands.Find(id);
            var aa = db.Brands.Where(x => x.Id == id).Select(x => new
            {
                x.BrandName,
            });
            if (brand == null)
            {
                return Ok(new
                {
                    result = "無此品牌"
                });
            }
            return Ok(aa);
        }

        //上傳餐車資料照片
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("BrandInfo/upload")]
        [JwtAuthFilter]

        public HttpResponseMessage PostUploadImage()
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int id = Convert.ToInt32(jwtAuthUtil.GetId(token));
            Brand brand = db.Brands.Find(id);

            try
            {
                var file = HttpContext.Current.Request.Files.Count > 0
                    ? HttpContext.Current.Request.Files[0]
                    : null;

                if (file != null && file.ContentLength > 0)
                {
                    //新的檔案名稱
                    string fileName = Utility.UploadImage(file);


                    //產生圖片連結
                    UriBuilder uriBuilder = new UriBuilder(HttpContext.Current.Request.Url)
                    {
                        Path = $"/Upload/brand/info{fileName}"
                    };

                    Uri imageUrl = uriBuilder.Uri;
                    brand.LogoPhoto = imageUrl.ToString();
                    brand.CarImage = imageUrl.ToString();
                    brand.QrCode = imageUrl.ToString();

                    db.Entry(brand).State = EntityState.Modified;
                    db.SaveChanges();


                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        success = true,
                        message = "上傳成功",
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




        // PUT: api/Brands/5
            [ResponseType(typeof(void))]
        public IHttpActionResult PutBrand(int id, Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != brand.Id)
            {
                return BadRequest();
            }

            db.Entry(brand).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BrandExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        // DELETE: api/Brands/5
        [ResponseType(typeof(Brand))]
        public IHttpActionResult DeleteBrand(int id)
        {
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return NotFound();
            }

            db.Brands.Remove(brand);
            db.SaveChanges();

            return Ok(brand);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BrandExists(int id)
        {
            return db.Brands.Count(e => e.Id == id) > 0;
        }
    }
}