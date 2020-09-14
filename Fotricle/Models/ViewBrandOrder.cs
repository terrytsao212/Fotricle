using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Fotricle.Models
{
    public class ViewBrandOrder
    {
        [Display(Name = "目前單號")]
        [DataType(DataType.Text)]
        public string OrderNumber { set; get; }//"?"為允許空值，日期數字才可使用問號

        [Display(Name = "取餐單號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MealNumber { set; get; }//"?"為允許空值，日期數字才可使用問號

        [Display(Name = "產品名稱")]
        public string ProductName { set; get; }

        [Display(Name = "產品單價")]
        public int ProductPrice { set; get; }

        [Display(Name = "產品數量")]
        public int ProductUnit { set; get; }

        [Display(Name = "總金額")]
        public int Amount { set; get; }

        [Display(Name = "LinePay驗證碼")]
        public string LinepayVer { set; get; }

        [Display(Name = "是否為現場單")]
        public Site Site { get; set; }


    }
}