using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using MailSender_Pattern_MVVM.DB_Elements;

namespace MailSender_Pattern_MVVM.Service
{
    public class EmailRecipientDataAccessService : IEmailRecipientDataAccessService
    {
        /// <summary>
        /// Class fields
        /// </summary>
        private readonly DatabaseContainer _emailRecipientDataContext;
        /// <summary>
        /// Constructor without parametrs
        /// </summary>
        public EmailRecipientDataAccessService()
        {
            _emailRecipientDataContext = new DatabaseContainer();
        }
        /// <summary>
        /// Creating new EmailRecipient record in DB
        /// </summary>
        /// <param name="emailRecipient"></param>
        /// <returns></returns>
        public bool CreateEmailRecipient(EmailRecipient emailRecipient)
        {
            try
            {
                var nEmail = _emailRecipientDataContext.EmailRecipients.FirstOrDefault(e => e.Email == emailRecipient.Email);

                if (nEmail != null)
                {
                    MessageBox.Show($@"Record with email ""{emailRecipient.Email}"" already exist");
                    return false;
                }

                _emailRecipientDataContext.EmailRecipients.Add(emailRecipient);
                _emailRecipientDataContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }
        /// <summary>
        /// Deleting existing record EmailRecipient from DB
        /// </summary>
        /// <param name="emailRecipient"></param>
        /// <returns></returns>
        public bool DeleteEmailRecipient(EmailRecipient emailRecipient)
        {
            try
            {
                _emailRecipientDataContext.EmailRecipients.Attach(emailRecipient);
                _emailRecipientDataContext.EmailRecipients.Remove(emailRecipient);
                _emailRecipientDataContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }
        /// <summary>
        /// Editing existiong record EmailRecipient in DB
        /// </summary>
        /// <param name="emailRecipient"></param>
        /// <returns></returns>
        public bool EditEmailRecipient(EmailRecipient emailRecipient)
        {
            try
            {
                EmailRecipient nEmailSmtp = _emailRecipientDataContext.EmailRecipients.Single(e => e.Id == emailRecipient.Id);
                nEmailSmtp.Email = emailRecipient.Email;
                nEmailSmtp.Name = emailRecipient.Name;
                nEmailSmtp.Address = emailRecipient.Address;
                _emailRecipientDataContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }
        /// <summary>
        /// Returning ObservableCollection<EmailRecipient/> from DB
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ObservableCollection<EmailRecipient> FindByName(string name) => 
            new ObservableCollection<EmailRecipient>(_emailRecipientDataContext.EmailRecipients.Where(e => e.Name.Contains(name)));
    }
}
