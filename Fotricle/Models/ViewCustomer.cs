using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fotricle.Models
{
    public class ViewCustomer
    {
        [Display(Name = "使用者暱稱")]
        [MaxLength(length: 50)]
        public string UserName { set; get; }

        [Display(Name = "客戶電話")]
        public string CusPhone { set; get; }

        [Display(Name = "性別")]
        public GenderType Gender { get; set; }

        [Display(Name = "年齡")]
        public string Age { set; get; }


    }
}