using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace YueDroidBox.ViewModel.Validation
{
    public class PortValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var flag = int.TryParse((string) value, out var result);

            if (flag && result > 0 && result <= 65535)
            {
                return ValidationResult.ValidResult;
            }

            return new ValidationResult(false, "Invalid Port");
        }
    }
}
