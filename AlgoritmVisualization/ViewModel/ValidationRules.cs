using System;
using System.Globalization;
using System.Windows.Controls;

namespace AlgorithmVisualization
{
    public class KeyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int res;
            if (MainWindow.IsDataAdded)
                MainWindow.IsDataAdded = false;
            else
            {
                if (String.IsNullOrWhiteSpace((value ?? "").ToString()))
                    return new ValidationResult(false, "Field is required.");
                if (!Int32.TryParse((String)value, out res))
                    return new ValidationResult(false, "Field should be integer value.");
            }
            return ValidationResult.ValidResult;
        }
    }

    public class DataValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (MainWindow.IsDataAdded)
                MainWindow.IsDataAdded = false;
            else
            {
                if (String.IsNullOrWhiteSpace((value ?? "").ToString()))
                    return new ValidationResult(false, "Field is required.");
            }
            return ValidationResult.ValidResult;
        }
    }
}
