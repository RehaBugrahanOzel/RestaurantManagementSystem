using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restaurant2.Models
{
    public class Cart
    {
        public int CartFoodId { get; set; }
        public String CartImage { get; set; }
        public String CartFoodName { get; set; }
        public String CartAbout { get; set; }
        public int CartPrice { get; set; }
        public int CartQuantity { get; set; }
        public int CartTotalPrice { get; set; } 
    }
}
