using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restaurant2.Models
{
    public class OrderInfo
    {
        public int OrderCartFoodId { get; set; }
        public String OrderCartImage { get; set; }
        public String OrderCartFoodName { get; set; }
        public String OrderCartAbout { get; set; }
        public int OrderCartPrice { get; set; }
        public int OrderCartQuantity { get; set; }
        public int OrderCartTotalPrice { get; set; }
        public int OrderCustomerId { get; set; }
        public String OrderCustomerName { get; set; }
        public String OrderCustomerLastName { get; set; }
        public String OrderCustomerEmail { get; set; }
        public String OrderCustomerPhoneNo { get; set; }
        public String OrderCustomerAddress { get; set; }
        public int OrderCustomerPaymentId { get; set; }
        public String OrderCustomerMessage { get; set; }
    }
}
