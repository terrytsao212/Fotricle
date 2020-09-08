using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Fotricle.Models
{
    public class OrderDetail
    {
        [Key]//指定這是裡面的主KEY
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        public int? OrderId { get; set; }

        [ForeignKey("OrderId")]
        [Display(Name = "訂單編號")]
        public virtual Order Order { set; get; }//virtual的意思是虛擬的


        public int? ProductListId { get; set; }

        [ForeignKey("ProductListId")]
        [Display(Name = "產品列表")]
        public virtual ProductList ProductList { set; get; }//virtual的意思是虛擬的

        [Display(Name = "產品名稱")]
        public string ProductName { set; get; }

        [Display(Name = "產品單價")]
        public int ProductPrice { set; get; }

        [Display(Name = "產品數量")]
        public int ProductUnit { set; get; }

        [Display(Name = "訂單小計")]
        public int Amount { set; get; }

        [Display(Name = "建立時間")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? InitDate { set; get; }//"?"為允許空值，日期數字才可使用問號




    }
}