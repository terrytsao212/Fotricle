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
    public class BrandCartController : ApiController
    {

        //新增現場單購物車
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("BrandCart/add")]
        [JwtAuthFilter]
        public IHttpActionResult BrandPostCart(ViewBrandCart viewBrandCart)
        {
            var Product = db.ProductLists.Find(viewBrandCart.ProductListId);
            if (Product == null)
            {
                return NotFound();
            }

            //顧客資料
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int id = Convert.ToInt32(jwtAuthUtil.GetId(token));

            bool isCart = db.Carts
                .Where(c => c.BrandId == id && c.CustomerId==0)
                .Any(c => c.ProductListId == viewBrandCart.ProductListId);

            if (isCart) //購物車有相同的商品
            {
                Cart cart = db.Carts.First(c => c.BrandId == id && c.ProductListId == viewBrandCart.ProductListId);
                cart.ProductUnit += 1;
                cart.Amount = cart.ProductPrice * cart.ProductUnit;
            }
            else
            {
                //餐車業主
                var Brand = db.Brands.FirstOrDefault(b => b.Id == Product.BrandId);


            db.Carts.Add(new Cart
            {
                CustomerId = 666,
                BrandId = id,
                BrandName = Brand.BrandName,
                ProductListId = viewBrandCart.ProductListId,
                ProductName = Product.ProductName,
                ProductPrice = Product.Price,
                ProductUnit = viewBrandCart.ProductUnit,
                Amount = Product.Price * viewBrandCart.ProductUnit,

            });

            }

            db.SaveChanges();

            return Ok(new
            {
                result = true,
                message = "已加入購物車",
                carts = db.Carts
                    .Where(cart => cart.CustomerId == id)
                    .Select(cart => new
                    {
                        cart.Id,
                        cart.CustomerId,
                        cart.BrandId,
                        cart.BrandName,
                        ProductList = new
                        {
                            cart.ProductListId,
                            cart.ProductName,
                            cart.ProductUnit,
                            cart.ProductPrice,
                            cart.Amount,

                        }
                    })
            });
        }

        // 刪除現場單購物車
        [System.Web.Http.HttpDelete]
        [System.Web.Http.Route("BrandCart/{Id}")]
        //[JwtAuthFilter]
        public IHttpActionResult BrandDeleteCart(int id)
        {
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return NotFound();
            }

            db.Carts.Remove(cart);
            db.SaveChanges();

            return Ok(cart);
        }

        // 清空現場單購物車
        [System.Web.Http.HttpDelete]
        [System.Web.Http.Route("BrandCart/ALL")]
        [JwtAuthFilter]


        public IHttpActionResult BrandDeleteCartAll()
        {

            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int id = Convert.ToInt32(jwtAuthUtil.GetId(token));


            // Cart cart = db.Carts.Find(id);
            var carts = db.Carts.Where(c => c.BrandId == id);


            db.Carts.RemoveRange(carts);

            db.SaveChanges();

            return Ok(new
            {
                result = true,
                message = "已清空購物車"
            });
        }

        //Get顧客現場購物車資料
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("BrandCart/customer/{cartId}")]
        [JwtAuthFilter]

        public IHttpActionResult BrandGetCart(int cartId)
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int id = Convert.ToInt32(jwtAuthUtil.GetId(token));


            var carts = db.Carts.Where(cart => cart.BrandId == id)
                .Select(cart => new
                {
                    cart.Id,
                    cart.CustomerId,
                    cart.BrandId,
                    cart.BrandName,
                    ProductList = new
                    {
                        cart.ProductListId,
                        cart.ProductName,
                        cart.ProductUnit,
                        cart.Amount
                    }
                });
            return Ok(new
            {
                result = true,
                carts
            });


        }

        //修改現場購物車資料(cart流水Id)
        [System.Web.Http.HttpPatch]
        [JwtAuthFilter]
        [System.Web.Http.Route("BrandCart/Edit")]
        public IHttpActionResult BrandEditCart(string id, [Bind(Include = "Id,ProductUnit")] ViewBrandCart viewBrandCart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Cart cart = new Cart();
            int id_temp = Convert.ToInt32(id);
            cart = db.Carts.Where(c => c.Id == id_temp).FirstOrDefault();
            cart.ProductUnit = viewBrandCart.ProductUnit;
            cart.Amount = viewBrandCart.ProductUnit * cart.ProductPrice;
            //cart.Amount = viewCart.Amount;

            db.Entry(cart).State = EntityState.Modified;
            db.SaveChanges();

            return Ok(new
            {
                result = true,
                message = "購物車更新成功"
            });

        }









        private Model1 db = new Model1();

        // GET: api/BrandCart
        public IQueryable<Cart> GetCarts()
        {
            return db.Carts;
        }

        // GET: api/BrandCart/5
        [ResponseType(typeof(Cart))]
        public IHttpActionResult GetCart(int id)
        {
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return NotFound();
            }

            return Ok(cart);
        }

        // PUT: api/BrandCart/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCart(int id, Cart cart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cart.Id)
            {
                return BadRequest();
            }

            db.Entry(cart).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
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

        // POST: api/BrandCart
        [ResponseType(typeof(Cart))]
        public IHttpActionResult PostCart(Cart cart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Carts.Add(cart);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = cart.Id }, cart);
        }

        // DELETE: api/BrandCart/5
        [ResponseType(typeof(Cart))]
        public IHttpActionResult DeleteCart(int id)
        {
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return NotFound();
            }

            db.Carts.Remove(cart);
            db.SaveChanges();

            return Ok(cart);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CartExists(int id)
        {
            return db.Carts.Count(e => e.Id == id) > 0;
        }
    }
}