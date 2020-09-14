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
    public class MyFollowsController : ApiController
    {
        private Model1 db = new Model1();

        // GET: Get顧客追蹤
        [HttpGet]
        [Route("customer/myfollow")]
        [JwtAuthFilter]
        public IHttpActionResult GetMyFollow()
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int id = Convert.ToInt32(jwtAuthUtil.GetId(token));
            var myfollow = db.MyFollows.Where(m => m.CustomerId == id)
               .Select(m => new
               {
                   m.CustomerId,
                   m.BrandId,
                   m.BrandName
               });
            return Ok(new
            {
                result = true,
                myfollow
            });
        }
        // GET: api/MyFollows
        //public IQueryable<MyFollow> GetMyFollows()
        //{
        //    return db.MyFollows;
        //}
        // GET: api/MyFollows/5
        [ResponseType(typeof(MyFollow))]
        public IHttpActionResult GetMyFollow(int id)
        {
            MyFollow myFollow = db.MyFollows.Find(id);
            if (myFollow == null)
            {
                return NotFound();
            }
            return Ok(myFollow);
        }
        // PUT: api/MyFollows/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMyFollow(int id, MyFollow myFollow)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != myFollow.Id)
            {
                return BadRequest();
            }
            db.Entry(myFollow).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MyFollowExists(id))
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
        //新增我的追蹤
        [HttpPost]
        [Route("myfollow/add")]
        [JwtAuthFilter]
        public IHttpActionResult PostMyFollow(MyFollow myFollow)
        {
            //顧客資料
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int id = Convert.ToInt32(jwtAuthUtil.GetId(token));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var checkMyfollow = db.MyFollows.FirstOrDefault(m => m.BrandId == myFollow.BrandId);
            if (checkMyfollow != null)
            {
                return Ok(new
                {
                    result = false,
                    message = "此餐車已經加入追蹤"
                });
            }
            var Brand = db.Brands.FirstOrDefault(b => b.Id == myFollow.BrandId);
            myFollow.CustomerId = id;
            myFollow.BrandName = db.Brands.FirstOrDefault(b => b.Id == myFollow.BrandId).BrandName;
            db.MyFollows.Add(myFollow);
            db.SaveChanges();
            return Ok(new
            {
                result = true,
                message = "已加入追蹤",
            });
        }
        //取消追蹤
        // DELETE: api/MyFollows/5
        [ResponseType(typeof(MyFollow))]
        [JwtAuthFilter]
        public IHttpActionResult DeleteMyFollow(int id)
        {
            MyFollow myFollow = db.MyFollows.Find(id);
            if (myFollow == null)
            {
                return NotFound();
            }
            db.MyFollows.Remove(myFollow);
            db.SaveChanges();
            return Ok(new
            {
                resutl = true,
                message = "已取消追蹤",
            });
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private bool MyFollowExists(int id)
        {
            return db.MyFollows.Count(e => e.Id == id) > 0;
        }
    }
}

