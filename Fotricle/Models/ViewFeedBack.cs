using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fotricle.Models
{
    public class ViewFeedBack
    {
        [Display(Name = "訂單編號")]
        public int? OrderId { get; set; }

        [Display(Name = "顧客編號")]
        public int CustomerId { get; set; }

        [Display(Name = "餐點滿意程度")]

        public string Food { set; get; }

        [Display(Name = "服務滿意程度")]
        public string Service { set; get; }

        [Display(Name = "整體滿意程度")]

        public string AllSuggest { set; get; }

        [Display(Name = "餐車建議")]
        public string CarSuggest { set; get; }
    }
}