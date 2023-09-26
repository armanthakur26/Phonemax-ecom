using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonemax.Models.Twilio
{
    public class Types
    {
        public class PhoneNumber : global::Twilio.Types.PhoneNumber
        {
            public PhoneNumber(string number) : base(number)
            {

            }
        }
    }
}
