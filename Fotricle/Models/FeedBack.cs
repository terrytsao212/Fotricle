using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using Newtonsoft.Json;

namespace Fotricle.Models
{
    public class FeedBack
    {
        [Key]//指定這是裡面的主KEY
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [Display(Name = "GUID")]
        public string Guid { set; get; }

        public int? OrderId { get; set; }

        [Display(Name = "訂單編號")]
        [ForeignKey("OrderId")]
        [JsonIgnore]
        public virtual Order Order { set; get; }//virtual的意思是虛擬的

        [Display(Name = "客戶編號")]
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        [JsonIgnore]
        public virtual Customer Customer { set; get; }

        [Display(Name = "餐點滿意程度")]
        
        public double Food { set; get; }

        [Display(Name = "服務滿意程度")]
        
        public double Service { set; get; }

        [Display(Name = "整體滿意程度")]
       
        public double AllSuggest { set; get; }

        [Display(Name = "餐車建議")]
        [MaxLength(length: 100)]//最大長度為200
        public string CarSuggest { set; get; }

        [Display(Name = "建立時間")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? InitDate { set; get; }//"?"為允許空值，日期數字才可使用問號

    }
}