using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fotricle.Models
{
    public class ViewOrder
    {
        [Display(Name = "品牌編號")]
        public int BrandId { get; set; }


        [Display(Name = "客戶編號")]
        public int CustomerId { get; set; }


        [Display(Name = "訂單日期")]
        public DateTime? OrderTime { set; get; }//"?"為允許空值，日期數字才可使用問號

        [Display(Name = "LinePay驗證碼")]
        public PaymentMethod Payment { get; set; }

        [Display(Name = "目前單號")]
        public string OrderNumber { set; get; }//"?"為允許空值，日期數字才可使用問號


        [Display(Name = "總金額")]
        public int Amount { set; get; }


        [Display(Name = "LinePay驗證碼")]
        public string LinepayVer { set; get; }


        [Display(Name = "是否為現場單")]
        public Site Site { get; set; }


        [Display(Name = "備註")]
        public string Remarks { set; get; }
    }
}