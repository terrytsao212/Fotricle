﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using Fotricle.Models;
using Fotricle.Security;
using WebGrease.Css.Extensions;

namespace Fotricle.Controllers
{
    public class BrandOrdersController : ApiController
    {
        private Model1 db = new Model1();

        // GET: api/BrandOrders
        public IQueryable<Order> GetOrders()
        {
            return db.Orders;
        }

        //新增餐車現場訂單
        [HttpPost]
        [Route("BrandOrder/add")]
        [JwtAuthFilter]
        public IHttpActionResult PostBrandOrder(ViewBrandOrder viewBrandOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int id = Convert.ToInt32(jwtAuthUtil.GetId(token));
            List<Brand> brands = db.Brands.Where(x => x.Id == id).ToList();
          //  List<Order> orders = db.Orders.Where(x => x.BrandId == id).ToList();
            List<Cart> carts = db.Carts.Where(x => x.BrandId == id).ToList();
            SqlConnection Conn = new SqlConnection();
            Conn.ConnectionString = ConfigurationManager.ConnectionStrings["Model1"].ConnectionString;
            DataTable dt = new DataTable();

            SqlCommand cmd = new SqlCommand("sp_InsertOrderNo", Conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@BrandId", SqlDbType.Int);
            cmd.Parameters["@BrandId"].Value = brands.Select(x => x.Id).FirstOrDefault();

            cmd.Parameters.Add("@CustomerId", SqlDbType.Int);
            cmd.Parameters["@CustomerId"].Value = 666;

            cmd.Parameters.Add("@Payment", SqlDbType.Int);
            cmd.Parameters["@Payment"].Value = 1;

            cmd.Parameters.Add("@OrderNumber", SqlDbType.NVarChar);
            cmd.Parameters["@OrderNumber"].Value = viewBrandOrder.OrderNumber;

            cmd.Parameters.Add("@Amount", SqlDbType.Int);
            cmd.Parameters["@Amount"].Value = viewBrandOrder.Amount;

            cmd.Parameters.Add("@LinepayVer", SqlDbType.NVarChar);
            cmd.Parameters["@LinepayVer"].Value = "現金";

            cmd.Parameters.Add("@Site", SqlDbType.Int);
            cmd.Parameters["@Site"].Value = 1;

            cmd.Parameters.Add("@OrderStatus", SqlDbType.Int);
            cmd.Parameters["@OrderStatus"].Value = 1;

            cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar);
            cmd.Parameters["@Remarks"].Value = viewBrandOrder.Remarks is null ? "" : viewBrandOrder.Remarks;

            SqlParameter OId = cmd.Parameters.Add("@Id", SqlDbType.Int);
            OId.Direction = ParameterDirection.Output;

            Conn.Open();
            cmd.ExecuteNonQuery();
            Conn.Close();

            foreach (var cart in carts)
            {
                OrderDetail orderDetail = new OrderDetail
                {
                    OrderId = Convert.ToInt32(OId.Value),
                    ProductListId = cart.ProductListId,
                    //ProductListId =db.ProductLists.Where(c=>c.Id==viewBrandOrder.ProductListId)
                    ProductName = cart.ProductName,
                    ProductPrice = cart.ProductPrice,
                    ProductUnit = cart.ProductUnit,
                    Amount = cart.Amount
                };

                db.OrderDetails.Add(orderDetail);
                //var orderssList = db.Orders.Where(x => x.Id == orderDetail.Id).ToList();
            }

            if (carts.Any())
            {
                db.Carts.RemoveRange(carts);
            }


            db.SaveChanges();

            return Ok(new
            {
                result = true,
                message = "已新增訂單"
            });

        }

        //拿餐車現場的取餐單號
        [HttpGet]
        //[JwtAuthFilter]
        [Route("BrandOrder/GetMeal")]
        public IHttpActionResult GetBrandOrder(int id)
        {
            //string token = Request.Headers.Authorization.Parameter;
            //JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            //int id = Convert.ToInt32(jwtAuthUtil.GetId(token));
            List<Order> orders = db.Orders.Where(o => o.BrandId == id && o.InitDate > DateTime.Today).ToList();
            var today = orders.Select(x => new
            {
                x.MealNumber,
                State = x.OrderStatus.ToString()
            }).ToList();

            return Ok(new
            {
                success = true,
                today,
            });
        }




        //拿餐車現場的訂單
        [HttpGet]
        [JwtAuthFilter]
        [Route("BrandOrder/Get")]
        public IHttpActionResult GetBrandOrder()
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int id = Convert.ToInt32(jwtAuthUtil.GetId(token));
            List<Order> orders = db.Orders.Where(o => o.BrandId == id && o.InitDate > DateTime.Today).ToList();
            var today = orders.Select(x => new
            {

                x.Id,
                x.CustomerId,
                status = x.OrderStatus.ToString(),
                x.OrderNumber,
                brandName = x.Brand.BrandName,
                x.LinepayVer,
                Total = x.OrderDetails.Sum(o => o.Amount),
                Site = x.Site.ToString(),
                x.OrderDetails

            }).ToList();

            return Ok(new
            {
                success = true,
                today,
            });
        }


        // GET: api/BrandOrders/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult GetOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // PUT: api/BrandOrders/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.Id)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/BrandOrders
        [ResponseType(typeof(Order))]
        public IHttpActionResult PostOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Orders.Add(order);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new {id = order.Id}, order);
        }

        // DELETE: api/BrandOrders/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult DeleteOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            db.Orders.Remove(order);
            db.SaveChanges();

            return Ok(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.Id == id) > 0;
        }


        //總銷售分析
        [HttpGet]
        [JwtAuthFilter]
        [Route("OrderSale/Get")]
        public IHttpActionResult GetOrderSale()
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int id = Convert.ToInt32(jwtAuthUtil.GetId(token));

            List<Order> orders = db.Orders.Where(o => o.BrandId == id).ToList();
            var idopen = db.OpenTimes.Where(x => x.BrandId == id && x.Status == OpenOrNot.營業中).ToList();

            var order = orders.Select(x => new
            {
                OrderId = x.Id,
                orders,
                BrandName = x.Brand.BrandName,
                time = Convert.ToDateTime(x.OrderTime).ToString("yyyy-MM-dd"),
                x.Amount,

            }).GroupBy(x => x.BrandName).Select(x => new
            {
                BrandName = x.Key,
                Time = x.GroupBy(y => y.time).Select(y => new
                {
                    Ordertime = y.Key,
                    total = y.Sum(a => a.Amount),
                    count = y.Count(),
                    workhour = WorkTime(y.Key, idopen),
                }),

            });


            return Ok(new
            {
                result = true,

                數據統計 = order

            });
        }


        public double WorkTime(string key, List<OpenTime> opensList)
        {
            //key = key.Replace("-", "");
            var open = opensList.Where(y => y.OpenDate.ToString("yyyy-MM-dd") == key).FirstOrDefault();
            if (open == null)
            {
                return 0;
            }

            double workHour = new TimeSpan(open.EDateTimeDate.Value.Ticks - open.SDateTime.Value.Ticks).TotalHours;
            return workHour;
        }


        [HttpGet]
        [Route("Brand/vip")]
        [JwtAuthFilter]
        public IHttpActionResult GetOrder()
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int id = Convert.ToInt32(jwtAuthUtil.GetId(token));
            SqlConnection Conn = new SqlConnection();
            Conn.ConnectionString = ConfigurationManager.ConnectionStrings["Model1"].ConnectionString;
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(@"select distinct ProductName from ProductLists where BrandId=" + id, Conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            string query = @"select *,{col1} as 總額
            from(select x.Orderdate,{col2}
            from(select *
                 from(
                     select Convert(varchar(10), o.OrderTime, 111) Orderdate, od.ProductName, od.Amount
                from Orders o inner
                join OrderDetails od
            on o.Id = od.OrderId
                )t
                pivot
            (
                sum(Amount)
            for ProductName in ({col3})
            )p
                )x)z";
            StringBuilder sbCol1 = new StringBuilder();
            foreach (DataRow item in dt.Rows)
            {
                sbCol1.Append($"z.{item[0]}+");
               
            }

            string col1 = sbCol1.ToString().TrimEnd('+');
            StringBuilder sbCol2 = new StringBuilder();
            foreach (DataRow item in dt.Rows)
            {
                sbCol2.Append($"isnull(x.{item[0]},0){item[0]},");
                
            }

            string col2 = sbCol2.ToString().TrimEnd(',');
            StringBuilder sbCol3 = new StringBuilder();
            foreach (DataRow item in dt.Rows)
            {
                sbCol3.Append($"[{item[0]}],");
                
            }

            string col3 = sbCol3.ToString().TrimEnd(',');
            query = query.Replace("{col1}", col1.ToString());
            query = query.Replace("{col2}", col2.ToString());
            query = query.Replace("{col3}", col3.ToString());

            DataTable vip = new DataTable();
            SqlCommand Cmd = new SqlCommand(query, Conn);
            SqlDataAdapter adapter1 = new SqlDataAdapter(Cmd);
            adapter1.Fill(vip);
            return Ok(new
            {
                vip
            });



        }

    }
}
