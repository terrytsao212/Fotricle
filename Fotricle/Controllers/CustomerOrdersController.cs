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
    public class CustomerOrdersController : ApiController
    {
        private Model1 db = new Model1();


        //新增訂單
        [HttpPost]
        [Route("order/add")]
        [JwtAuthFilter]
        public IHttpActionResult PostOrder(ViewOrder viewOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //顧客
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int id = Convert.ToInt32(jwtAuthUtil.GetId(token));
            List<Cart> carts = db.Carts.Where(x => x.CustomerId == id).ToList();
           
            if (carts.Count == 0)
            {
                return Ok(new
                {
                    result = false,
                    message = "購物車內無商品!"
                });
            }
            else if (carts.Where(x => x.CustomerId == id).GroupBy(x => x.BrandId).Count() > 1)
            {
                return Ok(new
                {
                    result = false,
                    message = "購物車內不能有2家商品!"
                });
            }


            SqlConnection Conn = new SqlConnection();
            Conn.ConnectionString = ConfigurationManager.ConnectionStrings["Model1"].ConnectionString;
            DataTable dt = new DataTable();

            SqlCommand cmd = new SqlCommand("sp_InsertOrderNo", Conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@BrandId", SqlDbType.Int);
            cmd.Parameters["@BrandId"].Value = carts.Select(x => x.BrandId).FirstOrDefault();

            cmd.Parameters.Add("@CustomerId", SqlDbType.Int);
            cmd.Parameters["@CustomerId"].Value = id;

            cmd.Parameters.Add("@Payment", SqlDbType.Int);
            cmd.Parameters["@Payment"].Value = viewOrder.Payment;

            cmd.Parameters.Add("@OrderNumber", SqlDbType.NVarChar);
            cmd.Parameters["@OrderNumber"].Value = viewOrder.OrderNumber;

            cmd.Parameters.Add("@Amount", SqlDbType.Int);
            cmd.Parameters["@Amount"].Value = viewOrder.Amount;

            cmd.Parameters.Add("@LinepayVer", SqlDbType.NVarChar);
            cmd.Parameters["@LinepayVer"].Value = viewOrder.LinepayVer;

            cmd.Parameters.Add("@Site", SqlDbType.Int);
            cmd.Parameters["@Site"].Value = 0;

            cmd.Parameters.Add("@OrderStatus", SqlDbType.Int);
            cmd.Parameters["@OrderStatus"].Value = 0;

            cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar);
            cmd.Parameters["@Remarks"].Value = viewOrder.Remarks is null ? "" : viewOrder.Remarks;

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
                    ProductName = cart.ProductName,
                    ProductPrice = cart.ProductPrice,
                    ProductUnit = cart.ProductUnit,
                    Amount = cart.Amount,
                };
                db.OrderDetails.Add(orderDetail);
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



        //Get顧客訂單資料
        [HttpGet]
        [JwtAuthFilter]
        [Route("customer/orders")]
        public IHttpActionResult GetCustomerOrders()
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int id = Convert.ToInt32(jwtAuthUtil.GetId(token));
            List<Order> orders = db.Orders.Where(o => o.CustomerId == id && o.InitDate > DateTime.Today).ToList();
            List<OrderDetail> orderDetails = db.OrderDetails.ToList();
            List<Brand> brands = db.Brands.ToList();
            var today = orders.Select(x => new
            {
                x.Id,
                status = x.OrderStatus.ToString(),
                x.OrderNumber,
                brandName = x.Brand.BrandName,
                x.LinepayVer,
                x.Remarks,
                x.Remark1,
                x.Remark2,
                x.Remark3,
                x.Remark4,
                Total = x.OrderDetails.Sum(o => o.Amount),
                x.OrderDetails
            }).ToList();
            return Ok(new
            {
                success = true,
                today,
            });
        }


        //更新訂單狀態
        [HttpPatch]
        [Route("update/orderstatus")]
        [JwtAuthFilter]
        public IHttpActionResult PostOrderStatus(ViewOrderStatus viewOrderStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var order = db.Orders.FirstOrDefault(o => o.Id == viewOrderStatus.OrderId);
            if (order == null)
            {
                return Ok(new
                {
                    result = false,
                    message = "查無此訂單",
                });
            }
            order.OrderStatus = viewOrderStatus.Status;
            order.Remark1 = viewOrderStatus.Remark1;
            order.Remark2 = viewOrderStatus.Remark2;
            order.Remark3 = viewOrderStatus.Remark3;
            order.Remark4 = viewOrderStatus.Remark4;
            order.CompleteTime = viewOrderStatus.CompleteTime;

            db.SaveChanges();
            return Ok(new
            {
                result = true,
                message = "已更新訂單狀態"
            });
        }

        // GET: api/CustomerOrders
        public IQueryable<Order> GetOrders()
        {
            return db.Orders;
        }

        // GET: api/CustomerOrders/5
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

        // PUT: api/CustomerOrders/5
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

        // POST: api/CustomerOrders
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

        // DELETE: api/CustomerOrders/5
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