using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace ConsoleApp10.SMTP
{
    internal class EmailSender
    {
        public void sendMail(string to, string subject, string content)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;

            smtpClient.Credentials = new NetworkCredential("yourmail@gmail.com", "XXXX XXXX XXXX XXXX"); // UPDATE GMAIL AND YOU APP KEY TO USE
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("yourmail@gmail.com"); // UPDATE GMAIL TO USE
            mailMessage.To.Add(to);
            mailMessage.Subject = subject;
            mailMessage.Body = content;

            smtpClient.Send(mailMessage);
        }
    }
}
