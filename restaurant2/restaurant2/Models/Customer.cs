using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restaurant2.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public String CustomerName { get; set; }
        public String CustomerLastName { get; set; }
        public String CustomerEmail { get; set; }
        public String CustomerPhoneNo { get; set; }
        public String CustomerAddress { get; set; }
        public int CustomerPaymentId { get; set; }
        public String CustomerMessage { get; set; }
    }
}
