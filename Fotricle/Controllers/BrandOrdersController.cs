using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Fotricle.Models;
using Fotricle.Security;

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
            List<Order> orders = db.Orders.Where(x => x.BrandId == id).ToList();
            SqlConnection Conn = new SqlConnection();
            Conn.ConnectionString = ConfigurationManager.ConnectionStrings["Model1"].ConnectionString;
            DataTable dt = new DataTable();

            SqlCommand cmd = new SqlCommand("sp_InsertOrderNo", Conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@BrandId", SqlDbType.Int);
            cmd.Parameters["@BrandId"].Value = brands.Select(x => x.Id).FirstOrDefault();

            cmd.Parameters.Add("@CustomerId", SqlDbType.Int);
            cmd.Parameters["@CustomerId"].Value =2;

            cmd.Parameters.Add("@Payment", SqlDbType.Int);
            cmd.Parameters["@Payment"].Value = viewBrandOrder.Payment;

            cmd.Parameters.Add("@OrderNumber", SqlDbType.NVarChar);
            cmd.Parameters["@OrderNumber"].Value = viewBrandOrder.OrderNumber;

            cmd.Parameters.Add("@Amount", SqlDbType.Int);
            cmd.Parameters["@Amount"].Value = viewBrandOrder.Amount;

            cmd.Parameters.Add("@LinepayVer", SqlDbType.NVarChar);
            cmd.Parameters["@LinepayVer"].Value = viewBrandOrder.LinepayVer;

            cmd.Parameters.Add("@Site", SqlDbType.Int);
            cmd.Parameters["@Site"].Value = viewBrandOrder.Site;

            cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar);
            cmd.Parameters["@Remarks"].Value = viewBrandOrder.Remarks is null ? "" : viewBrandOrder.Remarks;

            SqlParameter OId = cmd.Parameters.Add("@Id", SqlDbType.Int);
            OId.Direction = ParameterDirection.Output;
            Conn.Open();
            cmd.ExecuteNonQuery();
            Conn.Close();

            foreach (var order in orders)
            {
                OrderDetail orderDetail = new OrderDetail
                {
                    OrderId = Convert.ToInt32(OId.Value),
                    ProductListId = viewBrandOrder.ProductListId,
                    //ProductListId =db.ProductLists.Where(c=>c.Id==viewBrandOrder.ProductListId)
                    ProductName = viewBrandOrder.ProductName,
                    ProductPrice = viewBrandOrder.Price,
                    ProductUnit = viewBrandOrder.ProductUnit,
                    Amount = viewBrandOrder.Amount
                };

                db.OrderDetails.Add(orderDetail);

            }
            db.SaveChanges();
            return Ok(new
            {
                result = true,
                message = "已新增訂單"
            });

        }










        //拿餐車pos的訂單
        [HttpGet]
        //[JwtAuthFilter]
        [Route("BrandOrder/Get")]
        public IHttpActionResult GetBrandOrder(int id)
        {
            Brand brand = db.Brands.Find(id);
            var order = db.Orders.Where(c => c.Id == id).Select(c => new
            {
                c.Id,
                c.BrandId,
                c.CustomerId,
                c.OrderNumber,
                c.CompleteTime,
                c.Amount,
                c.LinepayVer,
                c.MealNumber,
                c.Payment,
                c.Remark1,
                c.Remark2,
                c.Remark3,
                c.Remark4,
                status = c.OrderStatus.ToString(),
                site = c.Site == Site.非現場 ? false : true,
                c.OrderTime,
                c.OrderDetails


            });
            return Ok(order);
        }
        //string token = Request.Headers.Authorization.Parameter;
        //JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
        //int id = Convert.ToInt32(jwtAuthUtil.GetId(token));
        //var order=db.OrderDetails.Join(db.Orders,c=>c.OrderId,o=>o.CustomerId,(c,o)=>c.Id)
        //{

        //        c.CustomerId,
        //        c.OrderNumber,
        //        c.CompleteTime,
        //        c.Amount,
        //        c.LinepayVer,
        //        c.MealNumber,
        //        c.OrderDetails,
        //        c.Payment,
        //        c.Remarks,
        //        status = c.OrderStatus.ToString(),
        //        site = c.Site == Site.非現場 ? false : true,
        //        c.OrderTime
        //}



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


    }
}