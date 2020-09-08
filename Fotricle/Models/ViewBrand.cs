using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fotricle.Models
{
    public class ViewBrand
    {
        [Display(Name = "品牌名稱")]
        [MaxLength(length: 50)]//指定後面nvchar為50
        public string BrandName { set; get; }

        [Display(Name = "品牌故事")]
        [MaxLength(length: 200)]
        public string BrandStory { set; get; }

        [Display(Name = "餐車電話")]
        [MaxLength(length: 50)]//最大長度為200
        public string PhoneNumber { set; get; }

        public ProductSort Sort { get; set; }

        [Display(Name = "LinePay轉帳代碼")]
        public string LinePay { set; get; }

        [Display(Name = "品牌Logo")]
        public string LogoPhoto { set; get; }

        [Display(Name = "品牌餐車照片")]
        public string CarImage { set; get; }

        [Display(Name = "LinePay QrCode")]
        public string QrCode { set; get; }

        [Display(Name = "審核業主帳號")]
        public Verification Verification { set; get; }

    }
}