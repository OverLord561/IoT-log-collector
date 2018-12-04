using IoTWebClient.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoTWebClient.Services
{
    public class AuthMessageSender : IEmailSender
    {
        private readonly IConfiguration _config;
        public AuthMessageSender(IConfiguration configuration)
        {
            //Options = optionsAccessor.Value;
            _config = configuration;
        }

        public SMSoptions Options { get; }  // set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }

        public async Task<int> SendSmsAsync(string number, string message)
        {
            ASPSMS.SMS SMSSender = new ASPSMS.SMS();

            //SMSSender.Userkey = Options.SMSAccountIdentification;
            //SMSSender.Password = Options.SMSAccountPassword;
            //SMSSender.Originator = Options.SMSAccountFrom;
            SMSSender.Userkey = _config.GetValue<string>("SMSAccountIdentification");
            SMSSender.Password = _config.GetValue<string>("SMSAccountPassword");
            SMSSender.Originator = _config.GetValue<string>("SMSAccountFrom");

            SMSSender.AddRecipient(number);
            SMSSender.MessageData = message;

            int x = await SMSSender.SendTextSMS();

            return x;
        }
    }
}
