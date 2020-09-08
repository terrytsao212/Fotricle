using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Fotricle.Models
{
    public class Address
    {
        [Key]//指定這是裡面的主KEY
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [ForeignKey("BrandID")]
        [Display(Name = "品牌編號")]
        public virtual Brand Brand { set; get; }//virtual的意思是虛擬的


        [Display(Name = "郵遞區號")]
        [MaxLength(length: 50)]
        public string PostalCode { set; get; }

        [Display(Name = "城市")]
        [MaxLength(length: 50)]
        [Required(ErrorMessage = "(0)必填")]//指定回饋報錯
        public string City { set; get; }

        [Display(Name = "地址")]
        [MaxLength(length: 100)]
        [Required(ErrorMessage = "(0)必填")]//指定回饋報錯
        public string MyAddress { set; get; }

        [Display(Name = "建立時間")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? InitDate { set; get; }

    }
}