using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fotricle.Models
{
    public class ViewNoitce
    {
        [Display(Name = "訂單編號")]
        public int OrderId { get; set; }
        [Display(Name = "客戶編號")]
        public int CustomerId { get; set; }
        [Display(Name = "訂單狀態")]
        public OrderStatus OrderStatus { get; set; }
        [Display(Name = "備註")]
        public string Remarks { set; get; }
        [Display(Name = "是否已讀")]
        public ReadOrNot IsRead { get; set; }



    }
}