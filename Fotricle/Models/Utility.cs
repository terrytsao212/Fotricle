using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using Jose;
using Newtonsoft.Json;

namespace Fotricle.Models
{
    public class Utility
    {
        #region JWT 驗證
        /// <summary>
        /// JWT 驗證
        /// </summary>
        /// <param name="id"></param>
        /// <returns>token</returns>
        public static string GenerateToken(int id)
        {
            string secret = "fotricle";//加解密的key,如果不一樣會無法成功解密
            Dictionary<string, Object> claim = new Dictionary<string, Object>();//payload 需透過token傳遞的資料
            claim.Add("Id", id);
            //  claim.Add("Permission", permission);
            claim.Add("iat", DateTime.Now.ToString());
            claim.Add("Exp", DateTime.Now.AddSeconds(Convert.ToInt32("86400")).ToString());//Token 時效設定100秒
            var payload = claim;
            var token = Jose.JWT.Encode(payload, Encoding.UTF8.GetBytes(secret), JwsAlgorithm.HS512);//產生token
            return token;
        }

        public static int GetId(string Token)
        {
            string secret = "fotricle";//加解密的key,如果不一樣會無法成功解密
            var jwtObject = Jose.JWT.Decode<Dictionary<string, Object>>(
                Token,
                Encoding.UTF8.GetBytes(secret),
                JwsAlgorithm.HS512);
            return Convert.ToInt32(jwtObject["Id"]);
        }

        #endregion


        #region GetParameter
        /// <summary>
        /// GetParameter
        /// </summary>
        /// <param name="token"></param>
        /// <returns>id</returns>
        public static int GetParameter(string token)
        {
            string secret = ConfigurationManager.AppSettings["secret"];
            var jwtObject = JWT.Decode<Dictionary<string, object>>(
                token,
                Encoding.UTF8.GetBytes(secret),
                JwsAlgorithm.HS256);
            int id = (int)jwtObject["Id"];
            return id;
        }
        #endregion



        #region "密碼加密"

        public const int DefaultSaltSize = 5;

        /// <summary>
        /// 產生Salt
        /// </summary>
        /// <returns>Salt</returns>
        public static string CreateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[DefaultSaltSize];
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }

        ///// <summary>
        ///// 密碼加密
        ///// </summary>
        ///// <param name="password">密碼明碼</param>
        ///// <returns>Hash後密碼</returns>
        public static string CreateHash(string password)
        {
            string salt = CreateSalt();
            string saltAndPassword = String.Concat(password, salt);
            string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPassword, "SHA1");
            hashedPassword = string.Concat(hashedPassword, salt);
            return hashedPassword;
        }
        /// <summary>
        /// Computes a salted hash of the password and salt provided and returns as a base64 encoded string.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="salt">The salt to use in the hash.</param>
        public static string GenerateHashWithSalt(string password, string salt)
        {
            // merge password and salt together
            string sHashWithSalt = password + salt;
            // convert this merged value to a byte array
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(sHashWithSalt);
            // use hash algorithm to compute the hash
            HashAlgorithm algorithm = new SHA256Managed();
            // convert merged bytes to a hash as byte array
            byte[] hash = algorithm.ComputeHash(saltedHashBytes);
            // return the has as a base 64 encoded string
            return Convert.ToBase64String(hash);
        }

        #endregion

        #region "將使用者資料寫入cookie,產生AuthenTicket"

        /// <summary>
        /// 將使用者資料寫入cookie,產生AuthenTicket
        /// </summary>
        /// <param name="userData">使用者資料</param>
        /// <param name="userId">UserAccount</param>
        static public void SetAuthenTicket(string userData, string userId)
        {
            //宣告一個驗證票
            FormsAuthenticationTicket ticket =
                new FormsAuthenticationTicket(1, userId, DateTime.Now, DateTime.Now.AddHours(3), false, userData);
            //加密驗證票
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            //建立Cookie
            HttpCookie authenticationcookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            //將Cookie寫入回應
            HttpContext.Current.Response.Cookies.Add(authenticationcookie);
        }


        //取得使用者資訊
        public ViewLogin GetUser()
        {
            //取得 ASP.NET 使用者
            var user = HttpContext.Current.User;

            //是否通過驗證
            if (user?.Identity?.IsAuthenticated == true)
            {
                //取得 FormsIdentity
                var identity = (FormsIdentity)user.Identity;

                //取得 FormsAuthenticationTicket
                var ticket = identity.Ticket;

                //將 Ticket 內的 UserData 解析回 User 物件
                return JsonConvert.DeserializeObject<ViewLogin>(ticket.UserData);
            }
            return null;
        }


        #endregion

        ////登出
        //public void SignOut()
        //{
        //    //移除瀏覽器的表單驗證
        //    FormsAuthentication.SignOut();
        //}

        #region"儲存上傳圖片"
        /// <summary>
        /// 儲存上傳圖片
        /// </summary>
        /// <param name="uploadFile">HttpPostedFile 物件</param>
        /// <returns>儲存檔名</returns>
        public static string UploadImage(HttpPostedFile uploadFile)
        {
            string fileName = Path.GetFileName(uploadFile.FileName);
            //取得副檔名
            string extension = fileName.Split('.')[fileName.Split('.').Length - 1];
            //新檔案名稱
            fileName = $"{DateTime.Now.ToString("yyyyMMddhhmmss")}.{extension}";
            string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Upload/customer"), fileName);
            uploadFile.SaveAs(path);
            return fileName;
        }
        #endregion
    }








}