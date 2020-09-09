using Fotricle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Fotricle.Controllers
{
    public class ValueController : ApiController
    {

        [JwtAuthFilter]
        [HttpGet]
        [Route("GetIdentity")]
        public IHttpActionResult GetIdentity()
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            string identity = jwtAuthUtil.GetIdentity(token);
            if (identity == "0")
            {
                return Ok(new
                {
                    result = true,
                    message = "顧客"
                });
            }
            else if (identity == "1")
            {
                return Ok(new
                {
                    result = true,
                    message = "餐車"
                });
            }
            return Ok(new
            {
                result = false,
                message = "其他"
            });
        }
    }
}
            

