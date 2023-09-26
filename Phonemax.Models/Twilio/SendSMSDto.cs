using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonemax.Models.Twilio
{
    public class SendSMSDto
    {
        public string MobileNumber { get; set; }
        public string Body { get; set; }
    }
}
