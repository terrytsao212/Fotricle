﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Fotricle.Models
{
    public class OpenTime
    {
        [Key]//指定這是裡面的主KEY
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        public int? BrandId { get; set; }

        [ForeignKey("BrandId")]
        [Display(Name = "品牌編號")]
        [JsonIgnore]
        public virtual Brand Brand { set; get; }//virtual的意思是虛擬

        [Display(Name = "營業日期")]
        public DateTime OpenDate { set; get; }// "?"為允許空值，日期數字才可使用問號

        [Display(Name = "日期")] //要存星期幾
        public string Date { set; get; }//"?"為允許空值，日期數字才可使用問號

        public OpenOrNot Status { get; set; }

        [Display(Name = "營業時間起")]
        public DateTime? SDateTime { set; get; }//"?"為允許空值，日期數字才可使用問號

        [Display(Name = "營業時間迄")]
        public DateTime? EDateTimeDate { set; get; }//"?"為允許空值，日期數字才可使用問號

        [Display(Name = "營業位置")]
        [MaxLength(length: 50)]//最大長度為200
        public string Location { set; get; }

        [Display(Name = "經度")]
        
        public string Longitude { set; get; }

        [Display(Name = "緯度")]
        public string Latitude { set; get; }

        [Display(Name = "建立時間")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? InitDate { set; get; }//"?"為允許空值，日期數字才可使用問號



    }
}