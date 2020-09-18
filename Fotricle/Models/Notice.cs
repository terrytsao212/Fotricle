using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Fotricle.Models
{
    public class Notice
    {
        [Key]//指定這是裡面的主KEY
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }
        public int? CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        [Display(Name = "客戶編號")]
        public virtual Customer Customer  { set; get; }

        public int? OrderId { get; set; }

        [ForeignKey("OrderId")]
        [Display(Name = "訂單編號")]
        public virtual Order Order { set; get; }

        public OrderStatus OrderStatus { get; set; }

        [Display(Name = "備註")]
        [MaxLength(length: 50)]//指定後面nvchar為50
        public string Remarks { set; get; }

        public ReadOrNot IsRead { get; set; }

        [Display(Name = "建立時間")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? InitDate { set; get; }


    }
}