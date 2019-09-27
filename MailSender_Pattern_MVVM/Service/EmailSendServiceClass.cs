using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using MailSender_Pattern_MVVM.DB_Elements;
using Xceed.Wpf.Toolkit;

namespace MailSender_Pattern_MVVM.Service
{
    public class EmailSendServiceClass
    {
        /// <summary>
        /// Class Fields
        /// </summary>
        #region Fields
        private readonly string _senderLogin;
        private readonly string _senderPass;
        private readonly string _strBody;
        private readonly string _strSubject;
        private readonly string _strSmtp;
        private readonly int _intSmtpPort;
        //private readonly int _intSmtpPort; //It is strange for me but it's work without port, and I have to find out how it's work (=
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="senderLogin"></param>
        /// <param name="senderPass"></param>
        /// <param name="strBody"></param>
        /// <param name="strSubject"></param>
        /// <param name="strSmtp"></param>
        /// <param name="intSmtpPort"></param>
        /// 
        #region Constructor
        public EmailSendServiceClass(string senderLogin, string senderPass, string strBody, string strSubject, string strSmtp, int intSmtpPort)
        {
            _senderLogin = senderLogin;
            _senderPass = senderPass;
            _strBody = strBody;
            _strSubject = strSubject;
            _strSmtp = strSmtp;
            _intSmtpPort = intSmtpPort;
            //_intSmtpPort = intSmtpPort;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sending mait from senders E-mail to recipients E-mail
        /// </summary>
        /// <param name="recipientMail"></param>
        /// <returns></returns>
        private async Task SendMailAsync(string recipientMail)
        {
            if (string.IsNullOrEmpty(_senderLogin))
            {
                MessageBox.Show("Select sender and try again");
                return;
            }

            if (string.IsNullOrEmpty(recipientMail))
            {
                MessageBox.Show("Recipient E-mail is empty. Please check recipient E-mail and try again");
                return;
            }

            using (SmtpClient smtpClient = new SmtpClient())
            {
                var basicCredential = new NetworkCredential(_senderLogin, _senderPass);
                using (MailMessage message = new MailMessage())
                {
                    MailAddress fromAddress = new MailAddress(_senderLogin);

                    smtpClient.Host = _strSmtp;
                    //smtpClient.Port = _intSmtpPort; //Why is green? read above in same green comment (=
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = basicCredential;
                    smtpClient.EnableSsl = true;

                    message.From = fromAddress;
                    message.Subject = _strSubject;
                    message.IsBodyHtml = false;
                    message.Body = $@"{_strBody}";
                    message.To.Add(recipientMail);

                    try
                    {
                        await smtpClient.SendMailAsync(message).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Method run "SendMailAsync" method for all elements in collection
        /// </summary>
        /// <param name="emails"></param>
        /// <returns></returns>
        public async Task SendMailsAsync(ObservableCollection<EmailRecipient> emails) =>
            await Task.WhenAll(emails.Select(e => SendMailAsync(e.Email))).ConfigureAwait(false);

        /// <summary>
        /// Method run "SendMailAsync" method for all elements in collescion with DELAY
        /// </summary>
        /// <param name="emailRecipients"></param>
        /// <param name="selectedSchedulerDateTime"></param>
        /// <returns></returns>
        public async Task SendMailsAsyncScheduled(ObservableCollection<EmailRecipient> emailRecipients, DateTime selectedSchedulerDateTime)
        {
            await Task.Delay((int)(selectedSchedulerDateTime - DateTime.Now).TotalMilliseconds);
            await SendMailsAsync(emailRecipients);
        }
        #endregion
    }
}
