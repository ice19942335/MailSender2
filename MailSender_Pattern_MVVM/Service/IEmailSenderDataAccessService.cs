using System.Collections.ObjectModel;
using MailSender_Pattern_MVVM.DB_Elements;

namespace MailSender_Pattern_MVVM.Service
{
    public interface IEmailSenderDataAccessService
    {
        int CreateEmailSender(EmailSender email);
        bool DeleteEmailSender(EmailSender email);
        bool EditEmailSender(EmailSender email);
    }
}