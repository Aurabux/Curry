using System;
using System.Collections.Generic;
using System.Text;

namespace Curry.Models.ViewModels
{
    public class OrderDetailsCartVM
    {
        public OrderHeader OrderHeader { get; set; }
        public List<ShoppingCart> listCart { get; set; }
    }
}
