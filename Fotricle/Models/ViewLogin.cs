using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fotricle.Models
{
    public class ViewLogin
    {
        [Display(Name = "Email")]
        [MaxLength(100)]
        public string Email { get; set; }

        [Display(Name = "密碼")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


    }
}