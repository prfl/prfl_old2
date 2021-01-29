using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Profile.Shared.Models.Codgram;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace Profile.Server.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly CodgramContext _codgramContext;
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor, CodgramContext codgramContext)
        {
            Options = optionsAccessor.Value;
            _codgramContext = codgramContext;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public API SendGridAPI { get; set; }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SendGridKey, subject, message, email);
        }

        public async Task Execute(string apiKey, string subject, string message, string email)
        {
            SendGridAPI = await _codgramContext.API.FirstOrDefaultAsync(a => a.Entity == Entity.prfl && a.Provider == Provider.SendGrid);
            apiKey = SendGridAPI.APIKey;

            

            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("no-reply@prfl.ga", Options.SendGridUser),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            await client.SendEmailAsync(msg);
        }
    }
}