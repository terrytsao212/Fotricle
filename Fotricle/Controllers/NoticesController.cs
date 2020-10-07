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
    public class NoticesController : ApiController
    {
        private Model1 db = new Model1();

        //Get顧客通知
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("notice/customer")]
        [JwtAuthFilter]
        public IHttpActionResult GetCustomerNotice()
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int id = Convert.ToInt32(jwtAuthUtil.GetId(token));
            var notice = db.Notices.Where(n => n.Order.CustomerId == id && n.IsRead == 0).Select(n => new
            {
                n.Id,
                n.OrderId,
                n.Order.Brand.BrandName,
                n.Remarks,
                Status = n.OrderStatus.ToString(),
                n.InitDate,
            });
            return Ok(new
            {
                result = true,
                notice
            });
        }
        //新增顧客通知
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("notice/create")]
        [JwtAuthFilter]
        public IHttpActionResult PostNotice(ViewNoitce viewNoitce)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int id = Convert.ToInt32(jwtAuthUtil.GetId(token));
            var brand = db.Brands.FirstOrDefault(b => b.Id == id);
            var orders = db.Orders.FirstOrDefault(o => o.Id == viewNoitce.OrderId);

            Notice notice = new Notice();

            notice.CustomerId = orders.CustomerId;
            notice.OrderId = viewNoitce.OrderId;
            notice.OrderStatus = viewNoitce.OrderStatus;
            notice.Remarks = viewNoitce.Remarks;
            notice.IsRead = 0;
            db.Notices.Add(notice);
            db.SaveChanges();
            return Ok(new
            {
                result = true,
                message = "通知訊息已新增成功!"
            });
        }
        //修改顧客訊息通知是否已讀
        [System.Web.Http.HttpPatch]
        [System.Web.Http.Route("notice/update")]
        public IHttpActionResult PatchBrands(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Notice notice = new Notice();
            notice = db.Notices.FirstOrDefault(x => x.Id == id);
            notice.IsRead = ReadOrNot.是;
            db.Entry(notice).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(new
            {
                result = true,
                message = "修改成功"
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
        private bool NoticeExists(int id)
        {
            return db.Notices.Count(e => e.Id == id) > 0;
        }














    }
}