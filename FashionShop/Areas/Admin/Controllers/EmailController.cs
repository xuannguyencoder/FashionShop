using FashionShop.Models.Common;
using FashionShop.Models.ViewModel;
using OpenPop.Mime;
using OpenPop.Pop3;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace FashionShop.Areas.Admin.Controllers
{
    public class EmailController : BaseController
    {
        // GET: Admin/Email
        private Pop3Client pop3Client = new Pop3Client();

        public ActionResult Index()
        {
            SetupPop3Client();
            int count = pop3Client.GetMessageCount(); //total count of email in MessageBox
            var Emails = new List<EmailViewModel>(); //POPEmail type
            for (int i = count; i >= 1; i--)
            {
                Message message = pop3Client.GetMessage(i);
                string messID = ConvertData.MD5Hash(message.Headers.MessageId);
                EmailViewModel email = new EmailViewModel()
                {
                    MessageID = messID,
                    Subject = message.Headers.Subject,
                    DateSent = message.Headers.DateSent
                };
                Emails.Add(email);
            }
            var emails = Emails;
            ViewBag.CountMess = count;
            return View(emails);
        }

        public ActionResult Compose()
        {
            SetupPop3Client();
            ViewBag.CountMess = pop3Client.GetMessageCount();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Compose(EmailViewModel email)
        {
            if (ModelState.IsValid)
            {
                var result = MailHelper.SendMail(email.Subject, email.Body, email.To);
                if (result)
                {
                    TempData["Message"] = "Gửi email thành công";
                    TempData["Status"] = "success";
                    return RedirectToAction("Compose");
                }
            }
            SetupPop3Client();
            ViewBag.CountMess = pop3Client.GetMessageCount();
            return View(email);
        }

        public ActionResult ReadEmail(string MessID)
        {
            SetupPop3Client();
            int count = pop3Client.GetMessageCount(); //total count of email in MessageBox
            ViewBag.CountMess = count;
            EmailViewModel email = null;
            bool flag = false;
            for (int i = count; i >= 1; i--)
            {
                Message message = pop3Client.GetMessage(i);
                string hashMessID = ConvertData.MD5Hash(message.Headers.MessageId);
                if (hashMessID == MessID)
                {
                    email = new EmailViewModel()
                    {
                        MessageID = hashMessID,
                        Subject = message.Headers.Subject,
                        DateSent = message.Headers.DateSent,
                        //From = string.Format("<a href = 'mailto:{1}'>{0}</a>",
                        //    message.Headers.From.DisplayName, message.Headers.From.Address),
                        From = message.Headers.From.Address,
                        Name = message.Headers.From.DisplayName
                    };
                    flag = true;
                    MessagePart body = message.FindFirstHtmlVersion();
                    if (body != null)
                    {
                        email.Body = body.GetBodyAsText();
                    }
                    else
                    {
                        body = message.FindFirstPlainTextVersion();
                        if (body != null)
                        {
                            email.Body = body.GetBodyAsText();
                        }
                    }
                    List<MessagePart> attachments = message.FindAllAttachments();

                    foreach (MessagePart attachment in attachments)
                    {
                        email.Attachments.Add(new Attachment
                        {
                            FileName = attachment.FileName,
                            ContentType = attachment.ContentType.MediaType,
                            Content = attachment.Body
                        });
                    }
                }
                if (flag)
                {
                    break;
                }
            }
            return View(email);
        }

        [ChildActionOnly]
        public ActionResult EmailLeftBox(int countMess = 0)
        {
            ViewBag.countMess = countMess;
            return PartialView();
        }

        private void SetupPop3Client()
        {
            pop3Client.Connect("pop.gmail.com", 995, true);
            pop3Client.Authenticate(
                ConfigurationManager.AppSettings["EmailAddress"].ToString(),
                ConfigurationManager.AppSettings["EmailPassword"].ToString(),
                AuthenticationMethod.UsernameAndPassword
            );
        }
    }
}