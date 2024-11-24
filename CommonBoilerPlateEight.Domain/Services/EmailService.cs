using MimeKit;
using CommonBoilerPlateEight.Domain.Exceptions;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models;
using MailKit.Net.Smtp;


namespace CommonBoilerPlateEight.Domain.Services
{
    public class EmailService: IEmailService
    {
        private readonly IApplicationSettingService _settingService;
        public EmailService(IApplicationSettingService settingService)
        {

            _settingService = settingService;

        }
        public async Task SendEmailAsync(SendEmailViewModel model)
        {
            var emailSetting = await _settingService.GetEmailSettings().ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(emailSetting.Host)) throw new CustomException("Email Setup Not Complete.");
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(emailSetting.FromName, emailSetting.FromEmail));
            foreach (var email in model.ToEmails)
            {
                emailMessage.To.Add(new MailboxAddress("", email));
            }

            emailMessage.Subject = model.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = model.Body };

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(emailSetting.Host, Convert.ToInt32(emailSetting.Port), MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(emailSetting.UserName, emailSetting.Password);
                await client.SendAsync(emailMessage);
            }
            catch (Exception ex)
            {
                // log ex but do not throw exception here
               Console.WriteLine("An error occurred while sending the email.");
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }
    }
}
