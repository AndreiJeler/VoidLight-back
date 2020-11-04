using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using VoidLight.Business.Services.Contracts;
using VoidLight.Data;
using VoidLight.Data.Entities;
using VoidLight.Infrastructure.Common;
using VoidLight.Infrastructure.Common.Exceptions;


namespace VoidLight.Business.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailAddress fromAddress;
        private readonly string fromPassword;
        private readonly AppSettings _appSettings;

        public EmailService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            fromAddress = new MailAddress(_appSettings.Email, "VoidLight");
            fromPassword = _appSettings.Password;
        }

        public async Task SendActivationEmail(User user, string token)
        {
            try
            {
                var emailContent = CreateAccountActivationEmail(user, token);
                var mail = ConfigureMailMessage(emailContent);
                var smtpServer = ConfigureStmpClient();

                await smtpServer.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                throw new SendEmailException(ex.Message, ex);
            }
        }

        public async Task SendResetPasswordEmail(User user, bool isForgotten)
        {
            try
            {
                var emailContent = CreateResetPasswordEmail(user, isForgotten);
                var mail = ConfigureMailMessage(emailContent);
                var smtpServer = ConfigureStmpClient();

                await smtpServer.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                throw new SendEmailException(ex.Message, ex);
            }
        }

        private Mail CreateAccountActivationEmail(User user, string token)
        {
            return new Mail
            {
                From = fromAddress,
                To = new MailAddress(user.Email, "New user"),
                Subject = Constants.ACCOUNT_ACTIVATION_SUBJECT,
                Body = Constants.ACCOUNT_ACTIVATION_BODY + Constants.ACCOUNT_ACTIVATION_LINK + token
            };
        }

        private Mail CreateResetPasswordEmail(User user, bool isForgotten)
        {
            var mail = new Mail
            {
                From = fromAddress,
                To = new MailAddress(user.Email, "Existing user"),
                Subject = Constants.RESET_PASSWORD_SUBJECT,
            };

            mail.Body = isForgotten ? mail.Body = Constants.TEMPORARY_PASSWORD_MESSAGE + user.Password : Constants.RESET_PASSWORD_MESSAGE;

            return mail;
        }

        private MailMessage ConfigureMailMessage(Mail email)
        {
            var mail = new MailMessage
            {
                From = email.From
            };
            mail.To.Add(email.To);
            mail.Subject = email.Subject;
            mail.Body = email.Body;

            return mail;
        }

        private SmtpClient ConfigureStmpClient()
        {
            var smtpServer = new SmtpClient(Constants.SMTP_CLIENT)
            {
                Port = Constants.SMTP_PORT,
                Credentials = new System.Net.NetworkCredential(fromAddress.Address, fromPassword),
                EnableSsl = true
            };

            return smtpServer;
        }
    }
}
