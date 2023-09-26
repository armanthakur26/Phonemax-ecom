using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonemax.dataaccess.Repository.Irepository
{
    public interface Iunitofwork
    {
        ICategoryrepostiory Category { get; }
        ICovertyperepostory Covertype { get; }
        Iprocessorrepository Processor { get; }
        Iproductrepository Product { get; }
        Icompany company { get; }
        Ishoppingcart shop{ get; }
        Iorderdetail orderdetail { get; }
        Iorderheader orderheader { get; }
        Iapplicationuserrepository applicationuser { get; }
       
        void save();
    }
}
