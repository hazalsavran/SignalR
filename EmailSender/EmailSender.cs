using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EmailSenderExample
{
    static class EmailSender
    {
        public static void Send(string to, string message)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;

            NetworkCredential credential = new NetworkCredential();//mail hesabı girilecek şifreyle
            smtpClient.Credentials = credential;
            MailAddress gonderen = new MailAddress("hazalsavran@gmail.com", "deneme");
            MailAddress alıcı = new MailAddress(to);

            MailMessage mail = new MailMessage(gonderen, alıcı);
            mail.Subject = "deneme";
            mail.Body = message;

            smtpClient.Send(mail);
        }
    }
}
