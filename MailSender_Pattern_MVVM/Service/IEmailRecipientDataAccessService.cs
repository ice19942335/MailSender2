using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailSender_Pattern_MVVM.DB_Elements;

namespace MailSender_Pattern_MVVM.Service
{
    public interface IEmailRecipientDataAccessService
    {
        bool CreateEmailRecipient(EmailRecipient emailRecipient);
        bool DeleteEmailRecipient(EmailRecipient emailRecipient);
        bool EditEmailRecipient(EmailRecipient emailRecipient);
        ObservableCollection<EmailRecipient> FindByName(string key);
    }
}
