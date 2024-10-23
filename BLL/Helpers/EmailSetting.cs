using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Helpers
{
    public static class EmailSetting
    {
        public static void SendEmail(Email email)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com",587);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(email.From, "jwdueoolpsmhinrn");
            smtpClient.Send(email.From,email.To,email.Title,email.Body);

        }
    }
}
