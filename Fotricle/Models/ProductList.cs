using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Fotricle.Models
{
    public class ProductList
    {
        [Key]//指定這是裡面的主KEY
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }


        public int? BrandId { get; set; }

        [ForeignKey("BrandId")]
        [JsonIgnore]
        [Display(Name = "品牌編號")]
        public virtual Brand Brand { set; get; }

        public ProductSort ProductSort { get; set; }

        [Display(Name = "產品名稱")]
        [MaxLength(length: 50)]//指定後面nvchar為50
        [Required(ErrorMessage = "(0)必填")]//指定回饋報錯
        public string ProductName { set; get; }

        [Display(Name = "產品單價")]
        [Required(ErrorMessage = "(0)必填")]//指定回饋報錯
        public int Price { set; get; }

        [Display(Name = "產品單位")]
        [Required(ErrorMessage = "(0)必填")]//指定回饋報錯
        public string Unit { set; get; }


        [Display(Name = "產品圖片")]
        public string ProductPhoto { set; get; }

        [Display(Name = "預銷總量")]
        public int Total { set; get; }

        [Display(Name = "產品描述")]
        [MaxLength(length: 100)]//指定後面nvchar為50
        [Required(ErrorMessage = "(0)必填")]//指定回饋報錯
        public string ProductDetail { set; get; }

        [Display(Name = "折扣")]
        public int Discount { set; get; }

        public Use IsUse { get; set; }

        [Display(Name = "建立時間")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? InitDate { set; get; }//"?"為允許空值，日期數字才可使用問號




    }
}