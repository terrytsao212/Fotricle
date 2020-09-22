using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using Fotricle.Models;
using Fotricle.Security;

namespace Fotricle.Controllers
{
    public class ProductListsController : ApiController
    {
        private Model1 db = new Model1();

        // GET: api/ProductLists
        public IQueryable<ProductList> GetProductLists()
        {
            return db.ProductLists;
        }

        // GET: api/ProductLists/5
        [ResponseType(typeof(ProductList))]
        public IHttpActionResult GetProductList(int id)
        {
            ProductList productList = db.ProductLists.Find(id);
            if (productList == null)
            {
                return NotFound();
            }

            return Ok(productList);
        }

        // PUT: api/ProductLists/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProductList(int id, ProductList productList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != productList.Id)
            {
                return BadRequest();
            }

            db.Entry(productList).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductListExists(id))
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

        //拿品牌的產品資料(整筆)
        
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("ProductLists/Gets")]
        public IHttpActionResult GetProducts(int id)
        {
            //string token = Request.Headers.Authorization.Parameter;
            //JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            //int id = Convert.ToInt32(jwtAuthUtil.GetId(token));
            Brand brand = db.Brands.Find(id);
            var products = db.ProductLists.Where(c => c.BrandId == id).Select(c => new
            {
                c.Id,
                c.BrandId,
                c.ProductName,
                c.ProductDetail,
                c.ProductPhoto,
                IsUse=c.IsUse.ToString(),
                c.Price,
                sort=c.ProductSort.ToString(),
                c.Total,
                c.Unit
            });
          
            return Ok(new {success = true, products});
        }

        // POST: 新增產品
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("ProductList/New")]

        public IHttpActionResult PostProductList(ProductList productList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ProductLists.Add(productList);
            db.SaveChanges();

            return Ok(new
            {

                result = true,
                message = "新增修改成功"
            });

            //CreatedAtRoute("ProductList/New", new { id = productList.Id }, productList);
        }

        // DELETE: 刪除產品
        [System.Web.Http.HttpDelete]
        [JwtAuthFilter]
        [System.Web.Http.Route("ProductList/Delete")]

        public IHttpActionResult DeleteProductList(int id)
        {
            //string token = Request.Headers.Authorization.Parameter;
            //JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            //int id = Convert.ToInt32(jwtAuthUtil.GetId(token));
            
            ProductList productList = db.ProductLists.Find(id);
            if (productList == null)
            {
                return NotFound();
            }

            db.ProductLists.Remove(productList);
            db.SaveChanges();

            return Ok(new
            {

                result = true,
                message = "產品刪除成功"
            });

        }

        // 修改產品資料
        [System.Web.Http.HttpPatch]
        [JwtAuthFilter]
        [System.Web.Http.Route("ProductList/Edit")]
        public IHttpActionResult EditProduct(string id, [Bind(Include = "Id,ProductName,ProductDetail,ProductPhoto,Unit,Price,ProductSort,Total")] ViewProducts viewProducts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ProductList product = new ProductList();
            int id_temp = Convert.ToInt32(id);
            product = db.ProductLists.Where(x => x.Id == id_temp).FirstOrDefault();
            product.ProductName = viewProducts.ProductName;
            product.ProductDetail = viewProducts.ProductDetail;
            product.ProductPhoto = viewProducts.ProductPhoto;
            product.ProductSort = viewProducts.ProductSort;
            product.Unit = viewProducts.Unit;
            product.Price = viewProducts.Price;
            product.Total = viewProducts.Total;

            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();

            return Ok(new
            {
                //productname=product.ProductSort.ToString(),
                result = true,
                message = "產品修改成功"
            });

        }

        //產品是否開啟
        [System.Web.Http.HttpPatch]
        [System.Web.Http.Route("Products/Use")]
        public IHttpActionResult UseProducts(int id, [Bind(Include = "Id,IsUse")] ViewProducts viewBool)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           // ViewProducts productsList =new ViewProducts();
            int id_temp = Convert.ToInt32(id);
            ProductList productsList = db.ProductLists.Where(x => x.Id==id_temp).FirstOrDefault();
            productsList.IsUse = viewBool.IsUse;
            db.Entry(productsList).State = EntityState.Modified;
            db.SaveChanges();
            if (productsList.IsUse== Use.否)
            {
                return Ok(new
                {
                    result = true,
                    message = "產品已關閉"
                });
            }
            else if (productsList.IsUse == Use.刪除)
            {
                return Ok(new
                {
                    resutl = true,
                    message = "產品已刪除"
                });
            }
            else
            {
                return Ok(new
                {
                    result = true,
                    message = "產品已啟用"
                });
            }
        }

        //拿產品資訊
        [System.Web.Http.HttpGet]
        [JwtAuthFilter]
        [System.Web.Http.Route("Products/Get")]
        public IHttpActionResult ProductGet(int? Id)
        {
            if (Id == null)
            {
                return Ok(new
                {
                    result = false,
                    message = "載入失敗"
                });
            }

            ProductList productsList = db.ProductLists.Find(Id);
            ViewProductBool product =new ViewProductBool();
            product.Id = productsList.Id;
            product.ProductName = productsList.ProductName;
            product.ProductSort = productsList.ProductSort;
            product.ProductDetail = productsList.ProductDetail;
            product.Price = productsList.Price;
            product.Unit = productsList.Unit;
            product.Total = productsList.Total;
            product.ProductPhoto = productsList.ProductPhoto;
            if (productsList.IsUse == Use.否)
                product.IsUse = false;
            else
                product.IsUse = true;
            //product.IsUse = productsList.IsUse  == Use.否? false :true ;

            return Ok(new
            {
                product,
                ProductSort=product.ProductSort.ToString(),
                productName = product.ProductSort.ToString(),
            });



        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("ProductPhoto/upload")]
        [JwtAuthFilter]

        public HttpResponseMessage PostUploadImage()
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int id = Convert.ToInt32(jwtAuthUtil.GetId(token));
            Brand brand = db.Brands.Find(id);

            try
            {
                var file = HttpContext.Current.Request.Files.Count > 0
                    ? HttpContext.Current.Request.Files[0]
                    : null;

                if (file != null && file.ContentLength > 0)
                {
                    //新的檔案名稱
                    string fileName = Utility.UploadProductImage(file);


                    //產生圖片連結
                    UriBuilder uriBuilder = new UriBuilder(HttpContext.Current.Request.Url)
                    {
                        Path = $"/Upload/brand/product/{fileName}"
                    };

                    Uri imageUrl = uriBuilder.Uri;
                    brand.LogoPhoto = imageUrl.ToString();
                    brand.CarImage = imageUrl.ToString();
                    brand.QrCode = imageUrl.ToString();

                    db.Entry(brand).State = EntityState.Modified;
                    db.SaveChanges();


                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        success = true,
                        message = "圖片上傳成功",
                        imageUrl

                    });

                }


                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    result = false,
                    message = "請選擇上傳圖片!"
                });

            }
            catch
            {
                throw;
            }
        }



        ////拿產品資訊(token)
        //[System.Web.Http.HttpGet]
        //[JwtAuthFilter]
        //[System.Web.Http.Route("Products/Gets")]
        //public HttpResponseMessage ProductGet(int Id)
        //{

        //    ProductList productsList = db.ProductLists.Find(Id);
        //    ViewProducts viewProducts = new ViewProducts();
        //    viewProducts.ProductName = productsList.ProductName;
        //    viewProducts.ProductSort = productsList.ProductSort;
        //    viewProducts.ProductDetail = productsList.ProductDetail;
        //    viewProducts.Price = productsList.Price;
        //    viewProducts.Unit = productsList.Unit;
        //    viewProducts.Total = productsList.Total;
        //    viewProducts.ProductPhoto = productsList.ProductPhoto;
        //    viewProducts.IsUse = productsList.IsUse == Use.是 ? true : false;

        //    JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
        //    string jwtToken= jwtAuthUtil.GenerateToken(productsList.Id);
        //    return Request.CreateResponse(HttpStatusCode.OK, new
        //    {
        //        result = true,
        //        message = "拿取資料成功",
        //        id = productsList.Id,
        //        token = jwtToken
        //    });



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductListExists(int id)
        {
            return db.ProductLists.Count(e => e.Id == id) > 0;
        }
    }
}