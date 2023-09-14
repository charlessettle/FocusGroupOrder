using System;
using System.Diagnostics;
using Api.Focus.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Api.Focus.Services
{
    public class TwilioService : ITwilioService
    {
        const string sendGridApiKey = "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";

        public TwilioService() { }

        public async Task<(string error, bool success)> SendEmailMessage(string email, int retry = 0, string msg = null)
        {
            try
            {
                var client = new SendGridClient(sendGridApiKey);
                var from = new EmailAddress("moreusedcars@live.com", "Charles Settle");
                var subject = "Time to select your items in a group order!";
                var to = new EmailAddress(email);
                string message = msg ?? $"{subject}.  You have been invited by {email}, Please open your app to complete your order!";
                var plainTextContent = message;
                var htmlContent = message; 
                var emailMsg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(emailMsg).ConfigureAwait(false);
                Debug.WriteLine($" success = {response.IsSuccessStatusCode}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.GetType()}, {ex.Message}");
                //try one more time in case of failutre
                if (retry == 0)
                {
                    await Task.Delay(300);
                    await SendEmailMessage(email, ++retry);
                }
                else
                    return (ex.Message, false);

            }
            return new("internal error occurred.", false);
        }
    }
}
