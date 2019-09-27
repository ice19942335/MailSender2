using System;
using System.Linq;
using System.Windows;
using MailSender_Pattern_MVVM.DB_Elements;

namespace MailSender_Pattern_MVVM.Service
{
    public class EmailSmtpDataAccessService : IEmailSmtpDataAccessService
    {
        //Field
        private readonly DatabaseContainer _emailSmtpDataContext;
        //Constructor
        public EmailSmtpDataAccessService()
        {
            _emailSmtpDataContext = new DatabaseContainer();
        }
        
        /// <summary>
        /// Creating new semtp record in DB
        /// </summary>
        /// <param name="emailSmtp"></param>
        /// <returns></returns>
        public bool CreateEmailSmtp(EmailSmtp emailSmtp)
        {
            try
            {
                _emailSmtpDataContext.EmailSmtps.Add(emailSmtp);
                _emailSmtpDataContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }
        /// <summary>
        /// Deleting existing record of smtp in DB
        /// </summary>
        /// <param name="emailSmtp"></param>
        /// <returns></returns>
        public bool DeleteEmailSmtp(EmailSmtp emailSmtp)
        {
            try
            {
                _emailSmtpDataContext.EmailSmtps.Attach(emailSmtp);
                _emailSmtpDataContext.EmailSmtps.Remove(emailSmtp);
                _emailSmtpDataContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }
        /// <summary>
        /// Editing existing record in DB
        /// </summary>
        /// <param name="emailSmtp"></param>
        /// <returns></returns>
        public bool EditEmailSmtp(EmailSmtp emailSmtp)
        {
            try
            {
                EmailSmtp nEmailSmtp = _emailSmtpDataContext.EmailSmtps.Single(e => e.Id == emailSmtp.Id);
                nEmailSmtp.SmtpServer = emailSmtp.SmtpServer;
                nEmailSmtp.Port = emailSmtp.Port;
                _emailSmtpDataContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }
    }
}
