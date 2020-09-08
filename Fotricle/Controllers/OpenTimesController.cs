﻿using System;
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

        // GET: api/OpenTimes
        public IQueryable<OpenTime> GetOpenTimes()
        {
            return db.OpenTimes;
        }

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

        public IHttpActionResult EditOpenTime(string id,
            [Bind(Include = "Id,Date,Status,SDateTime,EDateTimeDate,Location")]OpenTime openTime)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            OpenTime open=new OpenTime();
            int id_temp = Convert.ToInt32(id);
            open = db.OpenTimes.Where(x => x.Id == id_temp).FirstOrDefault();
            open.Date = openTime.Date;
            open.Location = openTime.Location;
            open.SDateTime = openTime.SDateTime;
            open.EDateTimeDate = openTime.EDateTimeDate;
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