using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FunctionApp
{
    public class Email
    {
        string _sender = "";
        string _password = "";
        public Email(string sender, string password)
        {
            _sender = sender;
            _password = password;
        }

        public bool SendMail(string recipient, string subject, string message)
        {
            SmtpClient client = new SmtpClient("smtp-mail.outlook.com");

            client.Port = 587;

            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential(_sender, _password);
            client.EnableSsl = true;
            client.Credentials = credentials;

            try
            {
                var mail = new MailMessage(_sender.Trim(), recipient.Trim());
                mail.Subject = subject;
                mail.Body = message;
                mail.Headers.Add("X-Priority", "3");
                mail.Headers.Add("X-MSMail-Priority", "Normal");
                mail.Headers.Add("X-Mailer", "Microsoft Outlook Express 6.00.2900.2869");
                mail.Headers.Add("X-MimeOLE", "Produced By Microsoft MimeOLE V6.00.2900.2869");
                mail.Headers.Add("ReturnReceipt", "1");
                client.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
