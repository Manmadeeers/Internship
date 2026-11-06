using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CRM.WebBlazor.Validator;

public partial class CodeAttribute:ValidationAttribute
{
    private static readonly Regex _regex = CodeRegex();

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrEmpty(value.ToString()))
        {
            return ValidationResult.Success;
        }

        if (_regex.IsMatch(value.ToString()!))
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }

    [GeneratedRegex(@"^\d{4}$")]
    private static partial Regex CodeRegex();
}