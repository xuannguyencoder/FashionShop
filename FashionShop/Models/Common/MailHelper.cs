using System;
using System.Configuration;
using System.Net.Mail;
using System.Text;

namespace FashionShop.Models.Common
{
    public class MailHelper
    {
        public static bool SendMail(string subjectText, string bodyText, string toEmail)
        {
            string From = ConfigurationManager.AppSettings["EmailAddress"];
            bool EnabledSSL = Boolean.Parse(ConfigurationManager.AppSettings["EnabledSSL"]);
            int Port = Int32.Parse(ConfigurationManager.AppSettings["SMTPPort"]);
            string Host = ConfigurationManager.AppSettings["SMTPHost"];
            string To = toEmail.Trim();
            string BCC = "";
            string CC = "";
            string subject = subjectText;

            StringBuilder sb = new StringBuilder();
            string Body = sb.Append(bodyText).ToString();

            var mail = SetUpMail(Body, From, To, subject, BCC, CC);
            var client = SetUpSmtp(Port, Host, EnabledSSL);
            try
            {
                client.Send(mail);
                return true;
            }
            catch
            {
            }
            return false;
        }

        private static MailMessage SetUpMail(string body, string from, string to, string subject, string bcc, string cc)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));
            if (!string.IsNullOrEmpty(bcc))
            {
                mail.Bcc.Add(new MailAddress(bcc));
            }
            if (!string.IsNullOrEmpty(bcc))
            {
                mail.CC.Add(new MailAddress(cc));
            }

            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            return mail;
        }

        private static SmtpClient SetUpSmtp(int Port, string Host, bool EnabledSSL)
        {
            SmtpClient client = new SmtpClient();
            client.Host = Host;
            client.Port = Port;
            client.EnableSsl = EnabledSSL;
            client.UseDefaultCredentials = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials =
            new System.Net.NetworkCredential(
                ConfigurationManager.AppSettings["EmailAddress"].ToString(),
                ConfigurationManager.AppSettings["EmailPassword"].ToString()
            );
            return client;
        }
    }
}