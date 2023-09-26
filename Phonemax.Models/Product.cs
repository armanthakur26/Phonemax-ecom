using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Phonemax.Models
{
    public class Product
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "Model Name")]
        public string Title { get; set; }
        [Required]
        public string Discription { get; set; }
        [Required]
        [Display(Name = "Ram")]
        public string Ram { get; set; }
        [Required]
        [Display(Name = "Storage Space")]
        public string Rom { get; set; }
        [Required]
        [Display(Name = "Battery Capacity")]
        public string Battery { get; set; }

        [Required]
        [Range(1, 10000000)]
        public double ListPrice { get; set; }
        [Required]
        [Range(1, 10000000)]
        public double Price { get; set; }
        [Required]
        [Range(1, 10000000)]
        public double Price50 { get; set; }
        [Required]
        [Range(1, 10000000)]
        public double Price100 { get; set; }

        [Display(Name = "Image Url")]
        public string ImageUrl { get; set; }

        [Display(Name = "Brand Name")]
        public int CategoryId { get; set; }
        public Category category { get; set; }

        [Display(Name = "Phone Model")]
        public int CoverTypeId { get; set; }
        public Covertype covertype { get; set; }
        [Display(Name = "Processor")]
        public int processorId { get; set; }
       public Processor processor { get; set; }
    }
}
