using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FashionShop.Models.ViewModel
{
    [Serializable]
    public class EmailViewModel
    {
        public EmailViewModel()
        {
            this.Attachments = new List<Attachment>();
        }
        public string Name { get; set; }
        public string MessageID { get; set; }
        [AllowHtml]
        public string To { get; set; }
        public string From { get; set; }
        [AllowHtml]
        public string Subject { get; set; }
        [AllowHtml]
        public string Body { get; set; }
        public DateTime DateSent { get; set; }
        [AllowHtml]
        public List<Attachment> Attachments { get; set; }
    }
    [Serializable]
    public class Attachment
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }
}