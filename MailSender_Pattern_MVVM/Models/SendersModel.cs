using System.Data.Entity;
using MailSender_Pattern_MVVM.DB_Elements;

namespace MailSender_Pattern_MVVM.Models
{
    public class SendersModel
    {
        private readonly DatabaseContainer _container = new DatabaseContainer();
        public DbSet<EmailSender> SendersEmails => _container.EmailSenders;
    }
}
