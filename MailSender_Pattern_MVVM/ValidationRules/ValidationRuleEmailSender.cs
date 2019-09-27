using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;



namespace MailSender_Pattern_MVVM.ValidationRules
{
    public class ValidationRuleEmailSender : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string data = value as string;
            if (!(value is string))
            {
                string s = "Entered data Error";
                return new ValidationResult(false, s);
            }

            if (data.Length > 100)
            {
                string s = "Entered E-mails is too long";
                return new ValidationResult(false, s);
            }

            if (!Regex.IsMatch(data, @"\w*[@]\w*[.]\w*"))
            {
                string s = "Entered E-mail is not correct";
                return new ValidationResult(false, s);
            }

            return ValidationResult.ValidResult;
        }
    }
}
