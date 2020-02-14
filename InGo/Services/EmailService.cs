using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using SmptClient = System.Net.Mail;

namespace InGo.Services
{
    public class EmailService
    {
        public void SendEmailAsync(string email, string subject, string body)
        {
            using (var message = new MailMessage())
            {
                const string fromEmail = "ingoboingoduolingo@gmail.com";
                const string fromPassword = "P1234567d";

                message.To.Add(new MailAddress(email, ""));
                message.From = new MailAddress(fromEmail, "Форум для стажёров Ингосстраха ingo.ru");
                //message.CC.Add(new MailAddress("cc@email.com", "CC Name"));
                //message.Bcc.Add(new MailAddress("bcc@email.com", "BCC Name"));
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                using (var client = new SmtpClient("smtp.gmail.com"))
                {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential(fromEmail, fromPassword);
                    client.EnableSsl = true;
                    client.Send(message);
                }
            }
        }
    }
}
