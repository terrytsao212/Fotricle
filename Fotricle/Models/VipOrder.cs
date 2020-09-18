using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Fotricle.Models
{
    public class VipOrder
    {
        [Key]//指定這是裡面的主KEY
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        public int BrandId { get; set; }

        [ForeignKey("BrandId")]
        [Display(Name = "品牌編號")]
        public virtual Brand Brand { set; get; }

        [Display(Name = "交易狀態")]
        public string Status { set; get; }

        [Display(Name = "交易訊息")]
        public string Message { set; get; }

        [Display(Name = "交易備註")]
        public string Remark { set; get; }

        [Display(Name = "建立時間")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? InitDate { set; get; }//"?"為允許空值，日期數字才可使用問號



    }
}