using System.Globalization;
using System.Windows.Controls;

namespace MailSender_Pattern_MVVM.ValidationRules
{
    public class ValidationRuleSmtpPort : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string data = value as string;
            if (!int.TryParse(data, out _)) return new ValidationResult(false, "Port can be only a number");
            return ValidationResult.ValidResult;
        }
    }
}
