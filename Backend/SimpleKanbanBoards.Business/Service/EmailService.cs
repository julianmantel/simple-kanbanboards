using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using SimpleKanbanBoards.Business.Models.Email;
using SimpleKanbanBoards.Business.Service.IService;

namespace SimpleKanbanBoards.Business.Service
{
    public class EmailService : IEmailService
    {

        private IConfiguration _configuration;
        
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task<string> SendEmailAsync(EmailRequestModel request)
        {
            string Result;

            try
            {
                using var smtp = new SmtpClient();
                smtp.Connect(_configuration["EmailSettings:EmailHost"], 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_configuration["EmailSettings:EmailUsername"], _configuration["EmailSettings:EmailPassword"]);

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:EmailUsername"]));
                email.To.Add(MailboxAddress.Parse(request.To));

                email.Subject = request.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = request.Message };

                smtp.Send(email);
                smtp.Disconnect(true);

                Result = "Sent Mail";
            }
            catch (Exception ex)
            {
                Result = $"Error sending email: {ex} ";
            }

            return Task.FromResult(Result);
        }
    }
}
