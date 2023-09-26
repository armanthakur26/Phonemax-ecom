using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonemax.Models.Viewmodel
{
    public class ProductVm
    {
        public Product product { get; set; }
        public IEnumerable<SelectListItem> categorylist { get; set; }
        public IEnumerable<SelectListItem> covertypelist { get; set; }
        public IEnumerable<SelectListItem> processorlist { get; set; }
    }
}
