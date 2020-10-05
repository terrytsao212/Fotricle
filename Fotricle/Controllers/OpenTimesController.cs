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
using System.Web.Mvc;
using Fotricle.Models;
using Fotricle.Security;

namespace Fotricle.Controllers
{
    public class OpenTimesController : ApiController
    {
        private Model1 db = new Model1();

        // GET: 拿個別營業資訊
        //[JwtAuthFilter]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("OpenTime/Get")]
        public IHttpActionResult GetOpen(int id)
        {
            //string token = Request.Headers.Authorization.Parameter;
            //JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            //int id = Convert.ToInt32(jwtAuthUtil.GetId(token));
          // Brand brand = db.Brands.Find(id);

          var open= db.OpenTimes.Where(o => o.BrandId == id).ToList();
          

            var result = new
            {
                open = open.Select(x => new
                {
                    x.Id,
                    x.BrandId,
                    x.Brand.BrandName,
                    OpenDate = Convert.ToDateTime(x.OpenDate).ToString("yyyy/MM/dd"),
                    SDateTime = Convert.ToDateTime(x.SDateTime).ToString("HH:mm"),
                    EDateTimeDate = Convert.ToDateTime(x.EDateTimeDate).ToString("HH:mm"),
                    x.Date,
                    Status = x.Status.ToString(),
                    x.Location,

                }),
            };

            return Ok(new 
                { success = true, 
                    result
                });

        }




        //public IQueryable<OpenTime> GetOpenTimes()
        //{
        //    return db.OpenTimes;
        //}

        // GET: api/OpenTimes/5
        [ResponseType(typeof(OpenTime))]
        public IHttpActionResult GetOpenTime(int id)
        {
            OpenTime openTime = db.OpenTimes.Find(id);
            if (openTime == null)
            {
                return NotFound();
            }

            return Ok(openTime);
        }

        // PUT: api/OpenTimes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOpenTime(int id, OpenTime openTime)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != openTime.Id)
            {
                return BadRequest();
            }

            db.Entry(openTime).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OpenTimeExists(id))
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
        // 修改營業時間
        [System.Web.Http.HttpPatch]
        [JwtAuthFilter]
        [System.Web.Http.Route("OpenTime/Edit")]

        public IHttpActionResult EditOpenTime(int id,
            [Bind(Include = "Id,Date,Status,SDateTime,EDateTimeDate,OpenDate,Location")]OpenTime openTime)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            OpenTime open=new OpenTime();
            open = db.OpenTimes.FirstOrDefault(x => x.Id == id);
            open.Date = openTime.Date;
            open.Location = openTime.Location;
            open.SDateTime = openTime.SDateTime;
            open.EDateTimeDate = openTime.EDateTimeDate;
            open.OpenDate = openTime.OpenDate;
            open.Status = openTime.Status;

            db.Entry(open).State = EntityState.Modified;
            db.SaveChanges();

            return Ok(new
            {
                result=true,
                message="營業時間更改成功"
            });



        }
        

        // POST: 新增營業時間
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("OpenTime/New")]
        public IHttpActionResult PostOpenTime(OpenTime openTime)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.OpenTimes.Add(openTime);
          
            db.SaveChanges();

            return Ok(new
            {
                result = true,
                message = "營業時間新增成功"
            });
            //CreatedAtRoute
            //("DefaultApi", new { id = openTime.Id }, openTime);
        }

        // DELETE: 刪除營業時間
        [System.Web.Http.HttpDelete]
        [System.Web.Http.Route("OpenTime/Delete")]
        public IHttpActionResult DeleteOpenTime(int id)
        {
            OpenTime openTime = db.OpenTimes.Find(id);
            if (openTime == null)
            {
                return NotFound();
            }

            db.OpenTimes.Remove(openTime);
            db.SaveChanges();

            return Ok(new
            {
                result = true,
                message = "營業時間刪除成功"
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

        private bool OpenTimeExists(int id)
        {
            return db.OpenTimes.Count(e => e.Id == id) > 0;
        }
    }
}