using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Fotricle.Models
{
    public class Cart
    {
        [Key]//指定這是裡面的主KEY
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }


        [Display(Name = "客戶編號")]
        public int CustomerId { get; set; }


        [Display(Name = "產品列表")]
        public int ProductListId { get; set; }


        [Display(Name = "品牌編號")]
        public int BrandId { get; set; }

        [Display(Name = "品牌名稱")]
        public string BrandName { get; set; }


        [Display(Name = "產品圖片")]
        public string ProductPhoto { set; get; }

        [Display(Name = "產品名稱")]
        public string ProductName { set; get; }

        [Display(Name = "產品數量")]
        public int ProductUnit { set; get; }

        [Display(Name = "產品單價")]
        public int ProductPrice { set; get; }

        [Display(Name = "金額小計")]
        public int Amount { set; get; }

        [Display(Name = "建立時間")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? InitDate { set; get; }//"?"為允許空值，日期數字才可使用問號

    }
}