using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restaurant2.Models
{
    public class OrderAdmin
    {
        public int OrderId { get; set; }
        public String CustomerInfo { get; set; }
        public String OrderInfo { get; set; }
        public int OrderPrice { get; set; }
    }
}
