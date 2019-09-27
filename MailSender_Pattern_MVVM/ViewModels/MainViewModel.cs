using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using CodePasswordDLL;
using DevExpress.Mvvm;
using MailSender_Pattern_MVVM.DB_Elements;
using MailSender_Pattern_MVVM.Models;
using MailSender_Pattern_MVVM.Service;
using MailSender_Pattern_MVVM.Views;
//using Xceed.Wpf.Toolkit;

namespace MailSender_Pattern_MVVM.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// USAGE: MainViewModel viewModel = MainViewModel.GetInstance(nameof(MainViewModel));
        /// This pattern I leave hire for some one who want to develop more this app. Usage inside
        /// Creating current object with all propertyes an e.t.c
        /// So you can call this obj from any class in this app.
        /// </summary>
        #region SingleTonePattern
        private static MainViewModel _instance;
        public string Name { get; private set; }
        private static readonly object SyncRoot = new Object();
        protected MainViewModel(string name)
        {
            this.Name = name;
        }
        public static MainViewModel GetInstance(string name)
        {
            if (_instance == null)
            {
                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new MainViewModel(name);
                }
            }
            return _instance;
        }

        #endregion 

        #region PrivateFields
        private string _subject = "MailSender _by Aleksejs Birula";
        //EmailSenders------------------------------------------------------------------------------------------------------------
        private readonly SendersModel _senders = new SendersModel(); //senders MODEL
        private EmailSenderDataAccessService _emailSenderDataAccessService = new EmailSenderDataAccessService();
        private EmailSender _selectedEmailSender = new EmailSender();
        //SMTP--------------------------------------------------------------------------------------------------------------------
        private readonly SmtpModel _smtp = new SmtpModel(); //Smtp MODEL
        private EmailSmtpDataAccessService _emailSmtpDataAccessService = new EmailSmtpDataAccessService();
        private EmailSmtp _selectedEmailSmtp = new EmailSmtp();
        //Recipients--------------------------------------------------------------------------------------------------------------
        private readonly RecipientsModel _recipients = new RecipientsModel(); //Recipients MODEL
        private EmailRecipientDataAccessService _emailRecipientDataAccessService = new EmailRecipientDataAccessService();
        private EmailRecipient _selectedEmailRecipient = new EmailRecipient();
        //FindByName--------------------------------------------------------------------------------------------------------------
        private string _findByName = string.Empty;

        #endregion

        #region Constructors
        /// <summary>
        /// Constructors
        /// </summary>
        public MainViewModel()
        {
            _instance = this;
        }
        #endregion

        #region Properties
        //EmailSender proprties----------------------------------------------------------------------------------------------------
        public ObservableCollection<EmailSender> EmailSenders => new ObservableCollection<EmailSender>(_senders.SendersEmails);
        public EmailSender EmailSenderInfo { get; set; } = new EmailSender();
        public EmailSender SelectedEmailSender
        {
            get => _selectedEmailSender;
            set
            {
                if (value == null) return;
                _selectedEmailSender = value;
                EmailSenderInfo = new EmailSender { Email = value.Email, Hash = String.Empty };
                RaisePropertyChanged(nameof(EmailSenderInfo));
                RaisePropertyChanged(nameof(SelectedEmailSender));
            }
        }
        //SMTP_server proprties----------------------------------------------------------------------------------------------------
        public ObservableCollection<EmailSmtp> EmailSmtps => new ObservableCollection<EmailSmtp>(_smtp.SendersSmtp);
        public EmailSmtp EmailSmtpInfo { get; set; } = new EmailSmtp();
        public EmailSmtp SelectedEmailSmtp
        {
            get => _selectedEmailSmtp;
            set
            {
                if (value == null) return;
                _selectedEmailSmtp = value;
                EmailSmtpInfo = new EmailSmtp { SmtpServer = value.SmtpServer, Port = value.Port };
                RaisePropertyChanged(nameof(EmailSmtpInfo));
                RaisePropertyChanged(nameof(SelectedEmailSmtp));
            }
        }
        //Recipients properties----------------------------------------------------------------------------------------------------
        public ObservableCollection<EmailRecipient> EmailRecipients
        {
            get
            {
                if (string.IsNullOrEmpty(FindByName))
                    return new ObservableCollection<EmailRecipient>(_recipients.Recipients);
                else
                    return new ObservableCollection<EmailRecipient>(_emailRecipientDataAccessService.FindByName(FindByName));
            }
        }
        public EmailRecipient EmailRecipientInfo { get; set; } = new EmailRecipient();
        public EmailRecipient SelectedEmailRecipient
        {
            get => _selectedEmailRecipient;
            set
            {
                if (value == null) return;
                _selectedEmailRecipient = value;
                EmailRecipientInfo = new EmailRecipient { Email = value.Email, Name = value.Name, Address = value.Address };
                RaisePropertyChanged(nameof(EmailRecipientInfo));
                RaisePropertyChanged(nameof(SelectedEmailRecipient));
            }
        }
        //FindByName property------------------------------------------------------------------------------------------------------
        public string FindByName
        {
            get => _findByName;
            set
            {
                _findByName = value;
                RaisePropertyChanged(nameof(FindByName));
                RaisePropertyChanged(nameof(EmailRecipients));
            }
        }
        //Scheduler properties-----------------------------------------------------------------------------------------------------
        public DateTime SelectedSchedulerDateTime { get; set; } = DateTime.Now;
        //RichTextBox Document text------------------------------------------------------------------------------------------------
        public string FlowDocumentFromRichTextBox { get; set; }
        //Views--------------------------------------------------------------------------------------------------------------------
        public string WarningText { get; set; } = "WarningText";
        public string ErrorText { get; set; } = "ErrorText";
        #endregion

        #region Commands
        public ICommand CloseAppCommand => new DelegateCommand(delegate { System.Windows.Application.Current.Shutdown(); });
        //EmailSenders ToolBar Commands--------------------------------------------------------------------------------------------
        public ICommand SaveEmailSenderCommand => new DelegateCommand(delegate { SaveEmailSender(EmailSenderInfo); });
        public ICommand DeleteEmailSenderCommand => new DelegateCommand(delegate { DeleteEmailSender(SelectedEmailSender); });
        public ICommand EditEmailSenderCommand => new DelegateCommand(delegate { EditEmailSender(SelectedEmailSender); });
        //Email SMTP Server ToolBar Commands---------------------------------------------------------------------------------------
        public ICommand SaveEmailSmtpCommand => new DelegateCommand(delegate { SaveEmailSmtp(EmailSmtpInfo); });
        public ICommand DeleteEmailSmtpCommand => new DelegateCommand(delegate { DeleteEmailSmtp(SelectedEmailSmtp); });
        public ICommand EditEmailSmtpCommand => new DelegateCommand(delegate { EditEmailSmtp(SelectedEmailSmtp); });
        //Email RecipientView Commands---------------------------------------------------------------------------------------------
        public ICommand SaveEmailRecipientCommand => new DelegateCommand(delegate { SaveEmailRecipient(EmailRecipientInfo); });
        public ICommand DeleteEmailRecipientCommand => new DelegateCommand(delegate { DeleteEmailRecipient(SelectedEmailRecipient); });
        public ICommand EditEmailRecipientCommand => new DelegateCommand(delegate { EditEmailRecipient(SelectedEmailRecipient); });
        //Send Commands------------------------------------------------------------------------------------------------------------
        public ICommand SendNowCommand => new DelegateCommand(async delegate
        {
            try
            {
                await SendNow(FlowDocumentFromRichTextBox);
            }
            catch (ArgumentNullException)
            {
                Warning("Please, check selected Senders E-mail and SMTP port");
            }

        });
        public ICommand SendToSelectedOnlyCommand => new DelegateCommand(async delegate
        {
            try
            {
                await SendToSelectedOnly(FlowDocumentFromRichTextBox);
            }
            catch (ArgumentNullException)
            {
                Warning("Please, select Sender E-mail and SMTP server.");
            }
        });
        public ICommand SendSheduledCommand => new DelegateCommand(async delegate
        {
            try
            {              
                await SendScheduled(FlowDocumentFromRichTextBox);
            }
            catch (ArgumentNullException)
            {
                Warning("Please, select Sender E-mail and SMTP server.");
            }
        });
        //Views Commands-----------------------------------------------------------------------------------------------------------
        public ICommand CloseWarningWindowCommand => new DelegateCommand(delegate {System.Windows.Application.Current.Shutdown();});
        #endregion

        #region Methods

        #region EmailSender methods
        /// <summary>
        /// Saying to save EmailSender in DB
        /// </summary>
        /// <param name="email"></param>
        private void SaveEmailSender(EmailSender email)
        {
            _emailSenderDataAccessService = new EmailSenderDataAccessService(); //For validation
            email.Hash = CodePassword.Encrypt(email.Hash);
            if (_emailSenderDataAccessService.CreateEmailSender(email) == 0) return;
            EmailSenders.Add(email);
            EmailSenderInfo = new EmailSender();
            SelectedEmailSender = null;
            RaisePropertyChanged(nameof(EmailSenders));
            RaisePropertyChanged(nameof(EmailSenderInfo));
            RaisePropertyChanged(nameof(SelectedEmailSender));
        }
        /// <summary>
        /// Saying to Delete EmailSender from DB
        /// </summary>
        /// <param name="email"></param>
        private void DeleteEmailSender(EmailSender email)
        {
            _emailSenderDataAccessService = new EmailSenderDataAccessService();
            if (!_emailSenderDataAccessService.DeleteEmailSender(email)) return;
            EmailSenders.Remove(email);
            EmailSenderInfo = new EmailSender();
            SelectedEmailSender = null;
            RaisePropertyChanged(nameof(EmailSenders));
            RaisePropertyChanged(nameof(EmailSenderInfo));
            RaisePropertyChanged(nameof(SelectedEmailSender));
        }
        /// <summary>
        /// Saying to update selected EmailSender in DB
        /// </summary>
        /// <param name="email"></param>
        private void EditEmailSender(EmailSender email)
        {
            _emailSenderDataAccessService = new EmailSenderDataAccessService();
            email.Hash = CodePassword.Encrypt(EmailSenderInfo.Hash);
            email.Email = EmailSenderInfo.Email;
            if (!_emailSenderDataAccessService.EditEmailSender(email)) return;
            EmailSenderInfo = new EmailSender();
            SelectedEmailSender = new EmailSender { Email = email.Email, Hash = email.Hash };
            RaisePropertyChanged(nameof(EmailSenders));
            RaisePropertyChanged(nameof(EmailSenderInfo));
            RaisePropertyChanged(nameof(SelectedEmailSender));
        }
        #endregion EmailSender methods

        #region EmailSMTP methods
        /// <summary>
        /// Saying to Save Smtp in DB
        /// </summary>
        /// <param name="smtp"></param>
        private void SaveEmailSmtp(EmailSmtp smtp)
        {
            _emailSmtpDataAccessService = new EmailSmtpDataAccessService(); //For Validation
            if (!_emailSmtpDataAccessService.CreateEmailSmtp(smtp)) return;
            EmailSmtpInfo = new EmailSmtp();
            RaisePropertyChanged(nameof(EmailSmtps));
            RaisePropertyChanged(nameof(EmailSmtpInfo));
            RaisePropertyChanged(nameof(SelectedEmailSmtp));
        }
        /// <summary>
        /// Saying to Delete Smtp from DB
        /// </summary>
        /// <param name="smtp"></param>
        private void DeleteEmailSmtp(EmailSmtp smtp)
        {
            _emailSmtpDataAccessService = new EmailSmtpDataAccessService();
            if (!_emailSmtpDataAccessService.DeleteEmailSmtp(smtp)) return;
            EmailSmtps.Remove(smtp);
            EmailSmtpInfo = new EmailSmtp();
            SelectedEmailSmtp = null;
            RaisePropertyChanged(nameof(EmailSmtps));
            RaisePropertyChanged(nameof(EmailSmtpInfo));
            RaisePropertyChanged(nameof(SelectedEmailSmtp));
        }
        /// <summary>
        /// Saying to Update Smtp in DB
        /// </summary>
        /// <param name="smtp"></param>
        private void EditEmailSmtp(EmailSmtp smtp)
        {
            _emailSmtpDataAccessService = new EmailSmtpDataAccessService();
            smtp.SmtpServer = EmailSmtpInfo.SmtpServer;
            smtp.Port = EmailSmtpInfo.Port;
            if (!_emailSmtpDataAccessService.EditEmailSmtp(smtp)) return;
            //EmailSmtpInfo = new EmailSmtp{SmtpServer = smtp.SmtpServer, Port = smtp.Port};
            EmailSmtpInfo = smtp;
            SelectedEmailSmtp = null;
            RaisePropertyChanged(nameof(EmailSmtps));
            RaisePropertyChanged(nameof(EmailSmtpInfo));
            RaisePropertyChanged(nameof(SelectedEmailSmtp));
        }
        #endregion EmailSMTP methods

        #region Email Recipients
        /// <summary>
        /// Saying to Save Recipient in DB
        /// </summary>
        /// <param name="recipient"></param>
        private void SaveEmailRecipient(EmailRecipient recipient)
        {
            if (string.IsNullOrEmpty(EmailRecipientInfo.Email)) return;
            _emailRecipientDataAccessService = new EmailRecipientDataAccessService(); //For validation
            if (!_emailRecipientDataAccessService.CreateEmailRecipient(recipient)) return;
            EmailRecipientInfo = new EmailRecipient();
            RaisePropertyChanged(nameof(EmailRecipients));
            RaisePropertyChanged(nameof(EmailRecipientInfo));
            RaisePropertyChanged(nameof(SelectedEmailRecipient));
        }
        /// <summary>
        /// Saying to Delete Recipient from DB
        /// </summary>
        /// <param name="recipient"></param>
        private void DeleteEmailRecipient(EmailRecipient recipient)
        {
            if (string.IsNullOrEmpty(SelectedEmailRecipient.Email)) return;
            _emailRecipientDataAccessService = new EmailRecipientDataAccessService();
            if (!_emailRecipientDataAccessService.DeleteEmailRecipient(recipient)) return;
            EmailRecipients.Remove(recipient);
            EmailRecipientInfo = new EmailRecipient();
            SelectedEmailRecipient = null;
            RaisePropertyChanged(nameof(EmailRecipients));
            RaisePropertyChanged(nameof(EmailRecipientInfo));
            RaisePropertyChanged(nameof(SelectedEmailRecipient));
        }
        /// <summary>
        /// Saying to Update Recipient in DB
        /// </summary>
        /// <param name="recipient"></param>
        private void EditEmailRecipient(EmailRecipient recipient)
        {
            if (string.IsNullOrEmpty(SelectedEmailRecipient.Email)) return;
            _emailRecipientDataAccessService = new EmailRecipientDataAccessService();
            recipient.Email = EmailRecipientInfo.Email;
            recipient.Name = EmailRecipientInfo.Name;
            recipient.Address = EmailRecipientInfo.Address;
            if (!_emailRecipientDataAccessService.EditEmailRecipient(recipient)) return;
            EmailRecipientInfo = new EmailRecipient();
            SelectedEmailRecipient = null;
            RaisePropertyChanged(nameof(EmailRecipients));
            RaisePropertyChanged(nameof(EmailRecipientInfo));
            RaisePropertyChanged(nameof(SelectedEmailRecipient));
        }
        #endregion Email Recipients

        #region SendMethod
        /// <summary>
        /// Asunc method saying to send message text to current list of emails in "EmailSenders" property
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        private async Task SendNow(string xmlString)
        {
            if (SelectedEmailSender == null || SelectedEmailSmtp == null)
            {
                Warning("You have to select Email sender and SMTP server and then try again");
                return;
            }
            if (xmlString == null)
            {
                Warning("Message text have to be a minimum one symbol please, check message text and try again");

                return;
            }
            var doc = (FlowDocument)XamlReader.Parse(xmlString);

            string stringToSend = new TextRange(doc.ContentStart, doc.ContentEnd).Text;
            string strSubject = _subject; // (=

            EmailSendServiceClass sender = new EmailSendServiceClass(
                SelectedEmailSender.Email,
                CodePassword.Decrypt(SelectedEmailSender.Hash),
                stringToSend,
                strSubject,
                SelectedEmailSmtp.SmtpServer,
                Int32.Parse(SelectedEmailSmtp.Port));

            await sender.SendMailsAsync(EmailRecipients);
            MessageBox.Show("E-mail sending compleated");
        }
        /// <summary>
        /// Asunc method saying to send message text to current "EmailRecipientInfo" property
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        private async Task SendToSelectedOnly(string xmlString)
        {
            if (SelectedEmailSender == null || SelectedEmailSmtp == null)
            {
                Warning("You have to select Email sender and SMTP server and then try again");
                return;
            }
            if (xmlString == null)
            {
                Warning("Message text have to be a minimum one symbol please, check message text and try again");
                return;
            }

            if (EmailRecipientInfo == null)
            {
                Warning("Please, select the recipient from list and try again");
                return;
            }
            var doc = (FlowDocument)XamlReader.Parse(xmlString);

            string stringToSend = new TextRange(doc.ContentStart, doc.ContentEnd).Text;
            string strSubject = _subject; // (=

            EmailSendServiceClass sender = new EmailSendServiceClass(
                SelectedEmailSender.Email,
                CodePassword.Decrypt(SelectedEmailSender.Hash),
                stringToSend,
                strSubject,
                SelectedEmailSmtp.SmtpServer,
                Int32.Parse(SelectedEmailSmtp.Port));

            ObservableCollection<EmailRecipient> recipientsColl = new ObservableCollection<EmailRecipient>();
            recipientsColl.Add(EmailRecipientInfo);

            await sender.SendMailsAsync(recipientsColl);
            MessageBox.Show("E-mail sending compleated");
        }
        /// <summary>
        /// Saying to send mail Scheduled to list of email in "EmailSenders" property
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        private async Task SendScheduled(string xmlString)
        {
            if (SelectedEmailSender == null || SelectedEmailSmtp == null)
            {
                Warning("You have to select Email sender and SMTP server and then try again");
                return;
            }
            if (xmlString == null)
            {
                Warning("Message text have to be a minimum one symbol please, check message text and try again");
                return;
            }

            if (EmailRecipientInfo == null)
            {
                Warning("Please, select the recipient from list and try again");
                return;
            }

            if (SelectedSchedulerDateTime < DateTime.Now)
            {
                Warning("Date and time have to be in future");
                return;
            }
            
            
            var doc = (FlowDocument)XamlReader.Parse(xmlString);

            string stringToSend = new TextRange(doc.ContentStart, doc.ContentEnd).Text;
            string strSubject = _subject; // (=

            EmailSendServiceClass sender = new EmailSendServiceClass(
                SelectedEmailSender.Email,
                CodePassword.Decrypt(SelectedEmailSender.Hash),
                stringToSend,
                strSubject,
                SelectedEmailSmtp.SmtpServer,
                Int32.Parse(SelectedEmailSmtp.Port));

            MessageBox.Show($"E-mail(s) will be send {SelectedSchedulerDateTime}");
            await sender.SendMailsAsyncScheduled(EmailRecipients, SelectedSchedulerDateTime);
            
        }
        #endregion SendMethod

        #region DialogsShow

        /// <summary>
        /// Creating dialog where is text with next instructions.
        /// </summary>
        /// <param name="message"></param>
        private void Warning(string message)
        {
            WarningText = message;
            WarningWindow warning = new WarningWindow();
            warning.ShowDialog();
        }

        #endregion

        #endregion Methods 
    }
}
