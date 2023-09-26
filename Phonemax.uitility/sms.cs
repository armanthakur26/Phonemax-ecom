using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Phonemax.uitility
{
    public class sms : Isms
    {
        private readonly Twilioset _twilioset;
        public sms(IOptions<Twilioset> twilioSet)
        {
            _twilioset = twilioSet.Value;
        }
        public async Task SendSmsAsync(string number, string message)
        {
            TwilioClient.Init(_twilioset.AccountSId, _twilioset.AuthToken);
            await MessageResource.CreateAsync(
                to: number,
                from: _twilioset.FromPhoneNumber,
                body: message
);
        }
    }
}
