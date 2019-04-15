using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SafestRouteApplication
{
    public static class SendAlert
    {
        public static void Send(string messagebody, string phoneNumber)
        {
            string accountSid = Keys.TwilioKey;//Load with things
            string authToken = Keys.TwilioToken;

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: messagebody,
                from: new Twilio.Types.PhoneNumber("+12628067443"),
                to: new Twilio.Types.PhoneNumber("+1" + phoneNumber)
            );

            Console.WriteLine(message.Sid);
        }
    }
}