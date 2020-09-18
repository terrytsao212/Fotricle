using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    public class FeedBacksController : ApiController
    {
        private Model1 db = new Model1();

        //Get 顧客回饋單
        [HttpGet]
        [Route("customer/feedback")]
        [JwtAuthFilter]
        public IHttpActionResult GetFeedBack()
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int id = Convert.ToInt32(jwtAuthUtil.GetId(token));

            var feedback = db.FeedBacks.Where(f => f.Order.BrandId == id)
                .Select(f => new
                {
                    
                    f.CustomerId,
                    f.Customer.UserName,
                    f.Customer.CusPhoto,
                    f.Order.BrandId,
                    f.Guid,
                    f.OrderId,
                    f.Food,
                    f.Service,
                    f.AllSuggest,
                    f.CarSuggest,
                });
            return Ok(new
            {
                result = true,
                feedback
            });

        }




        //新增回饋單
        [Route("feedback/create")]
        [JwtAuthFilter]
        public IHttpActionResult PostFeedBack(ViewFeedBack viewFeedBack)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int id = Convert.ToInt32(jwtAuthUtil.GetId(token));
            var orders = db.Orders.FirstOrDefault(o => o.CustomerId == id && o.OrderStatus == OrderStatus.訂單完成 && o.Id == viewFeedBack.OrderId);

            FeedBack feedBack = new FeedBack();

            if (orders == null)
            {
                return Ok(new
                {
                    result = false,
                    message = "訂單狀態尚未完成，無法填寫回饋單!"
                });
            }

            feedBack.CustomerId = id;
            string guid = Guid.NewGuid().ToString("N");
            feedBack.Guid = guid;
            feedBack.OrderId = viewFeedBack.OrderId;
            feedBack.Food = viewFeedBack.Food;
            feedBack.Service = viewFeedBack.Service;
            feedBack.AllSuggest = viewFeedBack.AllSuggest;
            feedBack.CarSuggest = viewFeedBack.CarSuggest;

            db.FeedBacks.Add(feedBack);
            db.SaveChanges();

            return Ok(new
            {
                result = true,
                message = "回饋單已填寫完成",

            });
        }





        // GET: api/FeedBacks
        public IQueryable<FeedBack> GetFeedBacks()
        {
            return db.FeedBacks;
        }

        // GET: api/FeedBacks/5
        [ResponseType(typeof(FeedBack))]
        public IHttpActionResult GetFeedBack(int id)
        {
            FeedBack feedBack = db.FeedBacks.Find(id);
            if (feedBack == null)
            {
                return NotFound();
            }

            return Ok(feedBack);
        }

        // PUT: api/FeedBacks/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFeedBack(int id, FeedBack feedBack)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != feedBack.Id)
            {
                return BadRequest();
            }

            db.Entry(feedBack).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedBackExists(id))
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

        // POST: api/FeedBacks
        [ResponseType(typeof(FeedBack))]
        public IHttpActionResult PostFeedBack(FeedBack feedBack)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FeedBacks.Add(feedBack);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = feedBack.Id }, feedBack);
        }

        // DELETE: api/FeedBacks/5
        [ResponseType(typeof(FeedBack))]
        public IHttpActionResult DeleteFeedBack(int id)
        {
            FeedBack feedBack = db.FeedBacks.Find(id);
            if (feedBack == null)
            {
                return NotFound();
            }

            db.FeedBacks.Remove(feedBack);
            db.SaveChanges();

            return Ok(feedBack);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FeedBackExists(int id)
        {
            return db.FeedBacks.Count(e => e.Id == id) > 0;
        }
    }
}