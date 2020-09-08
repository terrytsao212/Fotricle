using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fotricle.Models
{
    public enum VipMember
    {
        是 = 1,
        否 = 0,
    }

    public enum ProductSort
    {
        特色小吃 = 0,
        甜點 = 1,
        飲料 = 2,
        主食 = 3,
        炸物 = 4,
        素食 = 5,
        美式 = 6,
        日式 = 7,
        泰式 = 8,
    }

    public enum OpenOrNot
    {
        營業中 = 1,
        未營業 = 0,
    }

    public enum Verification
    {
        否 = 0,
        是 = 1,
    }

    public enum GenderType
    {
        男 = 0, //預設從0開始
        女 = 1,
        其他 = 2,
    }

    public enum OrderStatus
    {
        訂單處理中 = 0,
        訂單成立 = 1,
        訂單失敗 = 2,
        訂單餐點完成 = 3,
        訂單完成 = 4,
    }

    public enum PaymentMethod
    {
        LinePay = 0,
        現金 = 1,
    }

    public enum Site
    {
        否 = 0,
        是 = 1,
    }

    public enum Blacklist
    {
        否=0,
        是 = 1,
    }

    //public enum Star
    //{
    //    一星=1,
    //    二星=2,
    //    三星=3,
    //    四星=4,
    //    五星=5,
    //}

    public enum Use
    {
        否 = 0,
        是 = 1,
    }






 
}