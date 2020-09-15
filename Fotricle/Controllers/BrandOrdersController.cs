using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
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


        //拿餐車pos的訂單

        [HttpGet]
        [JwtAuthFilter]
        [Route("BrandOrder/Get")]
        public IHttpActionResult GetBrandOrder()
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int id = Convert.ToInt32(jwtAuthUtil.GetId(token));
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
            var order = db.Orders.Where(c => c.BrandId == id).Select(c => new
            {
                c.BrandId,
                c.CustomerId,
                c.OrderNumber,
                c.CompleteTime,
                c.Amount,
                c.LinepayVer,
                c.MealNumber,
                c.OrderDetails,
                c.Payment,
                c.Remarks,
                status = c.OrderStatus.ToString(),
                site = c.Site == Site.非現場 ? false : true,
                c.OrderTime

            });
            return Ok(new { success = true, order });

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

            return CreatedAtRoute("DefaultApi", new { id = order.Id }, order);
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
            var sales = db.Orders.Sum(c => c.Id);
            var amount = db.Orders.Select(c => c.Amount).Sum();
            var orderTime = db.Orders.Select(c => c.OrderTime).FirstOrDefault();
            //var openTime = db.OpenTimes.Where(c => EntityFunctions.DiffDays(c.EDateTimeDate, c.SDateTime) > 1).ToList();

            return Ok(new
            {
                sales,
                amount,
                orderTime,
                //openTime
            });
        }
        





    }
}