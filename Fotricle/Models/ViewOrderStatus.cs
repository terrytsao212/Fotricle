using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fotricle.Models
{
    public class ViewOrderStatus
    {
        public int OrderId { get; set; }
        public OrderStatus Status { get; set; }
    }
}