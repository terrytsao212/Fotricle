using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fotricle.Models
{
    public class ViewOrderStatus
    {
        public int OrderId { get; set; }
        public OrderStatus Status { get; set; }

        public string Remark1 { get; set; }
        public string Remark2 { get; set; }
        public string Remark3 { get; set; }
        public string Remark4 { get; set; }


        [Display(Name = "訂單完成時間")]
        public DateTime? CompleteTime { set; get; }//"?"為允許空值，日期數字才可使用問號


    }
}