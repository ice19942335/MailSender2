using System.Collections.ObjectModel;
using MailSender_Pattern_MVVM.DB_Elements;

namespace MailSender_Pattern_MVVM.Service
{
    public interface IEmailSmtpDataAccessService
    {
        bool CreateEmailSmtp(EmailSmtp emailSmtp);
        bool DeleteEmailSmtp(EmailSmtp emailSmtp);
        bool EditEmailSmtp(EmailSmtp emailSmtp);
    }
}
