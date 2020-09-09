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
    public class CartsController : ApiController
    {
        private Model1 db = new Model1();


        //新增購物車
        [HttpPost]
        [Route("cart/add")]
        [JwtAuthFilter]
        public IHttpActionResult PostCart(ViewCart viewCart)
        {
            var Product = db.ProductLists.Find(viewCart.ProductListId);
            if (Product == null)
            {
                return NotFound();
            }

            //顧客資料
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int id = Convert.ToInt32(jwtAuthUtil.GetId(token));

            bool isCart = db.Carts
                .Where(c => c.CustomerId == id)
                .Any(c => c.ProductListId == viewCart.ProductListId);

            if (isCart) //購物車有相同的商品
            {
                Cart cart = db.Carts.First(c => c.CustomerId == id && c.ProductListId == viewCart.ProductListId);
                cart.ProductUnit += 1;
                cart.Amount = cart.ProductPrice * cart.ProductUnit;
            }
            else
            {
                //餐車業主
                var Brand = db.Brands.FirstOrDefault(b => b.Id == Product.BrandId);

                var image = db.ProductLists.FirstOrDefault(p => p.Id == viewCart.ProductListId).ProductPhoto;

                db.Carts.Add(new Cart
                {
                    CustomerId = id,
                    BrandId = Brand.Id,
                    ProductListId = viewCart.ProductListId,
                    ProductName = Product.ProductName,
                    ProductPhoto = Product.ProductPhoto,
                    ProductPrice = Product.Price,
                    ProductUnit = viewCart.ProductUnit,
                    Amount = Product.Price * viewCart.ProductUnit,

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
                        ProductList = new
                        {
                            cart.ProductListId,
                            cart.ProductName,
                            cart.ProductUnit,
                            cart.ProductPrice,
                            cart.Amount,
                            cart.ProductPhoto
                        }
                    })
            });
        }



        // 刪除購物車
        [Route("cart/{Id}")]
        [JwtAuthFilter]
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

        // 清空購物車
        [Route("cart/ALL")]
        [JwtAuthFilter]


        public IHttpActionResult DeleteCartAll()
        {

            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int id = Convert.ToInt32(jwtAuthUtil.GetId(token));


            // Cart cart = db.Carts.Find(id);
            var carts = db.Carts.Where(c => c.CustomerId == id);


            db.Carts.RemoveRange(carts);
            // db.Carts.AddRange()
            //foreach (var c in carts)
            //{
            //    db.Carts.Remove(c);
            //}
            db.SaveChanges();

            return Ok(new
            {
                result = true,
                message = "已清空購物車"
            });
        }



        // GET: api/Carts
        public IQueryable<Cart> GetCarts()
        {
            return db.Carts;
        }

        // GET: api/Carts/5
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

        // PUT: api/Carts/5
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

        // POST: api/Carts
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

        //// DELETE: api/Carts/5
        //[ResponseType(typeof(Cart))]
        //public IHttpActionResult DeleteCart(int id)
        //{
        //    Cart cart = db.Carts.Find(id);
        //    if (cart == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Carts.Remove(cart);
        //    db.SaveChanges();

        //    return Ok(cart);
        //}

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