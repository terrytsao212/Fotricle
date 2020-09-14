using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Fotricle.Models
{
    public class Brand
    {
        [Key]//指定這是裡面的主KEY
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [Display(Name = "Email")]
        [MaxLength(length: 200)]//最大長度為200
        [Required(ErrorMessage = "(0)必填")]//指定回饋報錯
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "格式錯誤")]
        public string Email { set; get; }

        [Display(Name = "品牌密碼")]//{0}
        [StringLength(maximumLength: 100, ErrorMessage = "{0}長度至少必須為{2}個字，最大不能超過{1}個字", MinimumLength = 4)]//密碼長度必須為4個字
        [Required(ErrorMessage = "(0)必填")]//指定回饋報錯
        [DataType(DataType.Password)]
        public string Password { set; get; }

        [Display(Name = "品牌密碼鹽")]
        public string PasswordSalt { set; get; }

        [Display(Name = "品牌名稱")]
        [MaxLength(length:50)]//指定後面nvchar為50
        public string BrandName { set; get; }

        [Display(Name = "品牌故事")]
        [MaxLength(length: 200)]
        public string BrandStory { set; get; }

        [Display(Name = "餐車電話")]
        [MaxLength(length: 50)]//最大長度為200
        public string PhoneNumber { set; get; }

        [Display(Name = "營業地址")]
        [MaxLength(length: 100)]//最大長度為200
        public string Address { set; get; }

        //public virtual ICollection<Address> Addresses { set; get; }

        public VipMember Vip { get; set; }

        public ProductSort Sort { get; set; }

        [Display(Name = "品牌Logo")]
        public string LogoPhoto { set; get; }

        [Display(Name = "品牌餐車照片")]
        public string CarImage { set; get; }

        public OpenOrNot Status { get; set; }

        [Display(Name = "LinePay轉帳代碼")]
        public string LinePay { set; get; }

        [Display(Name = "LinePay QrCode")]
        public string QrCode { set; get; }

        [Display(Name = "品牌FB")]
        [Required(ErrorMessage = "(0)必填")]//指定回饋報錯
        [MaxLength(length: 100)]//最大長度為200
     
        public string FbAccount { set; get; }

        public Verification Verification { get; set; }

        [Display(Name = "餐車評分")]
        [MaxLength(length: 50)]//最大長度為200
        public string Score { set; get; }

        [Display(Name = "建立時間")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? InitDate { set; get; }//"?"為允許空值，日期數字才可使用問號

        [Display(Name = "權限")]
        [MaxLength(length: 50)]//最大長度為200
        public string Permission { set; get; }

    }
}