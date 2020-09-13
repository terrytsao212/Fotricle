using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fotricle.Models
{
    public class ViewCart
    {
        [Display(Name = "產品列表")]
        public int ProductListId { get; set; }


        [Display(Name = "產品數量")]

        public int ProductUnit { set; get; }


        [Display(Name = "金額小計")]
        public int Amount { set; get; }
    }
}