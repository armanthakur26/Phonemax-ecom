using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonemax.Models.Viewmodel
{
    public class ShoppingcartVm
    {
        public IEnumerable<Shoppingcart>listcart { get; set; }
        public Orderheader orderheader { get; set; }

    }
}
