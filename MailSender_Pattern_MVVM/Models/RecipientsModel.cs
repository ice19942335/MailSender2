using System.Data.Entity;
using MailSender_Pattern_MVVM.DB_Elements;

namespace MailSender_Pattern_MVVM.Models
{
    public class RecipientsModel
    {
        private readonly DatabaseContainer _container = new DatabaseContainer();
        public DbSet<EmailRecipient> Recipients => _container.EmailRecipients;
    }
}
