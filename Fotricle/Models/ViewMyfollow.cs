using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fotricle.Models
{
    public class ViewMyfollow
    {
        [Display(Name = "顧客編號")]
        public int CustomerId { get; set; }
        [Display(Name = "品牌編號")]
        public int BrandId { get; set; }
        [Display(Name = "品牌名稱")]
        public string BrandName { get; set; }



    }
}