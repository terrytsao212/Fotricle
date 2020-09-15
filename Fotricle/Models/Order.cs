using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Fotricle.Models
{
    public class Order
    {
        [Key]//指定這是裡面的主KEY
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        public int? BrandId { get; set; }

        [ForeignKey("BrandId")]
        [Display(Name = "品牌編號")]
        public virtual Brand Brand { set; get; }


        [Display(Name = "客戶編號")]
        public int? CustomerId  { set; get; }//virtual的意思是虛擬的

        public OrderStatus OrderStatus { get; set; }

        [Display(Name = "訂單日期")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime OrderTime { set; get; }//"?"為允許空值，日期數字才可使用問號

        public PaymentMethod Payment { get; set; }

        [Display(Name = "目前單號")]
        [DataType(DataType.Text)]
        public string OrderNumber { set; get; }//"?"為允許空值，日期數字才可使用問號

        [Display(Name = "取餐單號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int MealNumber { set; get; }//"?"為允許空值，日期數字才可使用問號

        [Display(Name = "總金額")]
        public int Amount { set; get; }

        [Display(Name = "LinePay驗證碼")]
        public string LinepayVer { set; get; }



        [Display(Name = "是否為現場單")]
        public Site Site { get; set; }

        [Display(Name = "訂單完成時間")]
        public DateTime? CompleteTime { set; get; }//"?"為允許空值，日期數字才可使用問號

        [Display(Name = "備註")]
        [MaxLength(length: 100)]//指定後面nvchar為50
        public string Remarks { set; get; }

        [Display(Name = "備註1")]
        [MaxLength(length: 100)]//指定後面nvchar為50
        public string Remark1 { set; get; }

        [Display(Name = "備註2")]
        [MaxLength(length: 100)]//指定後面nvchar為50
        public string Remark2 { set; get; }

        [Display(Name = "備註3")]
        [MaxLength(length: 100)]//指定後面nvchar為50
        public string Remark3 { set; get; }

        [Display(Name = "備註4")]
        [MaxLength(length: 100)]//指定後面nvchar為50
        public string Remark4 { set; get; }


        [Display(Name = "建立時間")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? InitDate { set; get; }//"?"為允許空值，日期數字才可使用問號

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }


    }
}