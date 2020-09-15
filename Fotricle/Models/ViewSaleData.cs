using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fotricle.Models
{
    public class ViewSaleData
    {
        [Display(Name = "日期")]
        public string Date { set; get; }//"?"為允許空值，日期數字才可使用問號

        [Display(Name = "訂單總數")]
        public int OrderId { set; get; }//"?"為允許空值，日期數字才可使用問號

        [Display(Name = "總營業時間")]
        public int AllOpenTime { set; get; }//"?"為允許空值，日期數字才可使用問號

        [Display(Name = "總金額")]
        public int Amounts { set; get; }



    }
}