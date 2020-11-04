using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace VoidLight.Infrastructure.Common
{
    public class Mail
    {
        public MailAddress From { get; set; }
        public MailAddress To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
