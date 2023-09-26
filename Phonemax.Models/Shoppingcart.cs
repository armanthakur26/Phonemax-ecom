using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonemax.Models
{
    public class Shoppingcart
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public Applicationuser applicationuser { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public int count { get; set; }
        [NotMapped]
        public double price { get; set; }
        public Shoppingcart()
        {
            count = 1;
        }
    }
}
