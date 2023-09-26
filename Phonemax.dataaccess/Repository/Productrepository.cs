using Phonemax.dataaccess.Data;
using Phonemax.dataaccess.Repository.Irepository;
using Phonemax.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonemax.dataaccess.Repository
{
    public class Productrepository:Repository<Product>,Iproductrepository
    {
        private readonly ApplicationDbContext _context;
        public Productrepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
    }
}
