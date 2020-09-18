using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Fotricle.Models;
using Fotricle.Models.NewbPay;
using Fotricle.Security;

namespace Fotricle.Controllers
{
    public class PayController : ApiController
    {
        private Model1 db = new Model1();

        private BankInfoModel _bankInfoModel = new BankInfoModel
        {
            MerchantID = "MS113893343",
            HashKey = "6gfdegvpE7wwJ2zKz9GpxxVCijHgFPvz",
            HashIV = "Cxyx7qJ1IdkQNyFP",
            ReturnURL = "http://yourWebsitUrl/Bank/SpgatewayReturn",
            NotifyURL = "http://yourWebsitUrl/Bank/SpgatewayNotify",
            CustomerURL = "http://yourWebsitUrl/Bank/SpgatewayCustomer",
            AuthUrl = "https://ccore.spgateway.com/MPG/mpg_gateway",
            CloseUrl = "https://core.newebpay.com/API/CreditCard/Close"
        };

        /// <summary>
        /// [智付通支付] 金流介接
        /// </summary>

        /// <returns></returns>
        [HttpPost]
        [JwtAuthFilter]
        [Route("GetBrand/Pay")]
        public IHttpActionResult SpgatewayPayBill(VipOrder vipOrder)
        {
            //var PayVip = db.VipOrders.Find(vipOrder.BrandId);
            //if (PayVip == null)
            //{
            //    return NotFound();
            //}

            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int id = Convert.ToInt32(jwtAuthUtil.GetId(token));

            // var payBrand = db.Brands.Where(b => b.Id == id);
            vipOrder.BrandId = id;
            db.VipOrders.Add(new VipOrder
            {
                BrandId = id,
            });

            db.SaveChanges();

            string version = "1.5";

            // 目前時間轉換 +08:00, 防止傳入時間或Server時間時區不同造成錯誤
            DateTimeOffset taipeiStandardTimeOffset = DateTimeOffset.Now.ToOffset(new TimeSpan(8, 0, 0));

            TradeInfo tradeInfo = new TradeInfo()
            {
                // * 商店代號
                MerchantID = _bankInfoModel.MerchantID,
                // * 回傳格式
                RespondType = "String",
                // * TimeStamp
                TimeStamp = taipeiStandardTimeOffset.ToUnixTimeSeconds().ToString(),
                // * 串接程式版本
                Version = version,
                // * 商店訂單編號
                //MerchantOrderNo = $"T{DateTime.Now.ToString("yyyyMMddHHmm")}",
                MerchantOrderNo = vipOrder.BrandId.ToString(),
                    // * 訂單金額
                Amt = 1200,
                // * 商品資訊
                ItemDesc = "數據分析功能" + vipOrder.BrandId.ToString(),
                // 繳費有效期限(適用於非即時交易)
                ExpireDate = null,
                // 支付完成 返回商店網址
                ReturnURL = _bankInfoModel.ReturnURL,
                // 支付通知網址
                NotifyURL = _bankInfoModel.NotifyURL,
                // 商店取號網址
                CustomerURL = _bankInfoModel.CustomerURL,
                // 支付取消 返回商店網址
                ClientBackURL = null,
                // * 付款人電子信箱
                Email = string.Empty,
                // 付款人電子信箱 是否開放修改(1=可修改 0=不可修改)
                EmailModify = 0,
                // 商店備註
                OrderComment = null,
                // 信用卡 一次付清啟用(1=啟用、0或者未有此參數=不啟用)
                CREDIT = null,
                // WEBATM啟用(1=啟用、0或者未有此參數，即代表不開啟)
                WEBATM = null,
                // ATM 轉帳啟用(1=啟用、0或者未有此參數，即代表不開啟)
                VACC = null,
                // 超商代碼繳費啟用(1=啟用、0或者未有此參數，即代表不開啟)(當該筆訂單金額小於 30 元或超過 2 萬元時，即使此參數設定為啟用，MPG 付款頁面仍不會顯示此支付方式選項。)
                CVS = null,
                // 超商條碼繳費啟用(1=啟用、0或者未有此參數，即代表不開啟)(當該筆訂單金額小於 20 元或超過 4 萬元時，即使此參數設定為啟用，MPG 付款頁面仍不會顯示此支付方式選項。)
                BARCODE = null
            };
            //暫定都信用卡
            tradeInfo.CREDIT = 1;
            //if (string.Equals(payType, "CREDIT"))
            //{
            //    tradeInfo.CREDIT = 1;
            //}
            //else if (string.Equals(payType, "WEBATM"))
            //{
            //    tradeInfo.WEBATM = 1;
            //}
            //else if (string.Equals(payType, "VACC"))
            //{
            //    // 設定繳費截止日期
            //    tradeInfo.ExpireDate = taipeiStandardTimeOffset.AddDays(1).ToString("yyyyMMdd");
            //    tradeInfo.VACC = 1;
            //}
            //else if (string.Equals(payType, "CVS"))
            //{
            //    // 設定繳費截止日期
            //    tradeInfo.ExpireDate = taipeiStandardTimeOffset.AddDays(1).ToString("yyyyMMdd");
            //    tradeInfo.CVS = 1;
            //}
            //else if (string.Equals(payType, "BARCODE"))
            //{
            //    // 設定繳費截止日期
            //    tradeInfo.ExpireDate = taipeiStandardTimeOffset.AddDays(1).ToString("yyyyMMdd");
            //    tradeInfo.BARCODE = 1;
            //}

            Atom<string> result = new Atom<string>()
            {
                IsSuccess = true
            };

            var inputModel = new SpgatewayInputModel
            {
                MerchantID = _bankInfoModel.MerchantID,
                Version = version
            };

            // 將model 轉換為List<KeyValuePair<string, string>>, null值不轉
            List<KeyValuePair<string, string>> tradeData = LambdaUtil.ModelToKeyValuePairList<TradeInfo>(tradeInfo);
            // 將List<KeyValuePair<string, string>> 轉換為 key1=Value1&key2=Value2&key3=Value3...
            var tradeQueryPara = string.Join("&", tradeData.Select(x => $"{x.Key}={x.Value}"));
            // AES 加密
            inputModel.TradeInfo =
                CryptoUtil.EncryptAESHex(tradeQueryPara, _bankInfoModel.HashKey, _bankInfoModel.HashIV);
            // SHA256 加密
            inputModel.TradeSha =
                CryptoUtil.EncryptSHA256(
                    $"HashKey={_bankInfoModel.HashKey}&{inputModel.TradeInfo}&HashIV={_bankInfoModel.HashIV}");

            // 將model 轉換為List<KeyValuePair<string, string>>, null值不轉
            List<KeyValuePair<string, string>> postData =
                LambdaUtil.ModelToKeyValuePairList<SpgatewayInputModel>(inputModel);



            return Ok(postData);
        }
        [Route("Notify")]
        [HttpPost]
        public IHttpActionResult SpgatewayNotify()
        {
            // 取法同SpgatewayResult
            var httpRequestBase = new HttpRequestWrapper(HttpContext.Current.Request);
            RazorExtensions.LogFormData(httpRequestBase, "SpgatewayNotify(支付完成)");
            // Status 回傳狀態 
            // MerchantID 回傳訊息
            // TradeInfo 交易資料AES 加密
            // TradeSha 交易資料SHA256 加密
            // Version 串接程式版本
            NameValueCollection collection = HttpContext.Current.Request.Form;

            if (collection["MerchantID"] != null && string.Equals(collection["MerchantID"], _bankInfoModel.MerchantID) &&
                collection["TradeInfo"] != null && string.Equals(collection["TradeSha"], CryptoUtil.EncryptSHA256($"HashKey={_bankInfoModel.HashKey}&{collection["TradeInfo"]}&HashIV={_bankInfoModel.HashIV}")))
            {
                var decryptTradeInfo = CryptoUtil.DecryptAESHex(collection["TradeInfo"], _bankInfoModel.HashKey, _bankInfoModel.HashIV);

                // 取得回傳參數(ex:key1=value1&key2=value2),儲存為NameValueCollection
                NameValueCollection decryptTradeCollection = HttpUtility.ParseQueryString(decryptTradeInfo);
                SpgatewayOutputDataModel convertModel = LambdaUtil.DictionaryToObject<SpgatewayOutputDataModel>(decryptTradeCollection.AllKeys.ToDictionary(k => k, k => decryptTradeCollection[k]));

                //LogUtil.WriteLog(JsonConvert.SerializeObject(convertModel));

                // TODO 將回傳訊息寫入資料庫

                VipOrder pay = db.VipOrders.Find(Convert.ToInt32(convertModel.MerchantOrderNo));
                pay.Message = convertModel.Message;
                pay.Status = convertModel.Status;
                db.VipOrders.Add(pay);
                // return Content(JsonConvert.SerializeObject(convertModel));

                if (pay.Status == "SUCCESS")
                {
                    //MerchantOrderNo = "O202009100034"
                    //convertModel.MerchantOrderNo
                    VipOrder vip = db.VipOrders.Find(pay.BrandId);
                    vip.Status = "已付款成功";
                   
                    db.Entry(vip).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else//付款失敗
                {

                }
                return Ok();
            }
            else
            {
                //LogUtil.WriteLog("MerchantID/TradeSha驗證錯誤");
            }

            return Ok();
        }



    }
}
