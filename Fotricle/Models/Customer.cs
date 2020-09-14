using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Fotricle.Models
{
    public class Customer
    {
        [Key]//指定這是裡面的主KEY
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }


        [Display(Name = "客戶帳號")]
        [MaxLength(length: 50)]//指定後面nvchar為50
        [Required(ErrorMessage = "(0)必填")]//指定回饋報錯
        [DataType(DataType.EmailAddress)]
        public string Email { set; get; }

        [Display(Name = "使用者暱稱")]
        [MaxLength(length: 50)]
        public string UserName { set; get; }

        [Display(Name = "客戶密碼")]//{0}
        [StringLength(maximumLength: 100, ErrorMessage = "{0}長度至少必須為{2}個字，最大不能超過{1}個字", MinimumLength = 4)]//密碼長度必須為4個字
        [Required(ErrorMessage = "(0)必填")]//指定回饋報錯
        [DataType(DataType.Password)]
        public string Password { set; get; }

        [Display(Name = "品牌密碼鹽")]
        public string PasswordSalt { set; get; }

        [Display(Name = "客戶電話")]
        public string CusPhone { set; get; }

        public GenderType Gender { get; set; }

        [Display(Name = "年齡")]
        public string Age { set; get; }

        [Display(Name = "個人照片")]
        public string CusPhoto { set; get; }

        [Display(Name = "棄單次數")]
        public string Abandon { set; get; }
        public Blacklist Blacklist { get; set; }

        [Display(Name = "建立時間")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? InitDate { set; get; }//"?"為允許空值，日期數字才可使用問號

    }
}