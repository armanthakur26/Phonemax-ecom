using Microsoft.CodeAnalysis.Operations;
using Phonemax.dataaccess.Data;
using Phonemax.dataaccess.Repository.Irepository;
using Phonemax.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Phonemax.dataaccess.Repository
{
    public class unitofwork:Iunitofwork
    {
        private readonly ApplicationDbContext _context;

        public unitofwork(ApplicationDbContext context)
        {
            _context = context;
            Category = new Categoryrepository(context);
            Covertype = new Covertyperepository(context);
            Processor=new processorrepository(context);
            Product=new Productrepository(context);
            company=new CompanyRepository(context);
            shop=new Shoppingcartrepository(context);
            orderdetail=new Orderdetailrepository(context);
            orderheader=new Orderheaderrepository(context);
            applicationuser=new Applicationuserrepository(context);
           
        }
        public  ICategoryrepostiory Category { get; private set; }
        public ICovertyperepostory Covertype { get; private set; }
        public Iprocessorrepository Processor { get; private set; }
        public Iproductrepository Product { get; private set; }
        public Icompany company { get; private set; }

        public Ishoppingcart shop { get; private set; }

        public Iorderdetail orderdetail { get; private set; }

        public Iorderheader orderheader { get; private set; }
        public Iapplicationuserrepository applicationuser { get; private set; }

        public void save()
        {
            _context.SaveChanges();
        }
    }
}
