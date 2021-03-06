﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Fotricle.Models
{
    public class MyFollow
    {
        [Key]//指定這是裡面的主KEY
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        public int? BrandId { get; set; }

        [ForeignKey("BrandId")]
        [Display(Name = "品牌編號")]
        public virtual Brand Brand { set; get; }

        [Display(Name = "品牌名稱")]
        [MaxLength(length: 50)]//指定後面nvchar為50
        public string BrandName { set; get; }

        public int? CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        [Display(Name = "客戶編號")]
        public virtual Customer Customer { set; get; }//virtual的意思是虛擬的

        [Display(Name = "建立時間")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? InitDate { set; get; }//"?"為允許空值，日期數字才可使用問號

    }
}