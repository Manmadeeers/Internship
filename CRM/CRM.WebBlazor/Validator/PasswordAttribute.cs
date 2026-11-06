using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CRM.WebBlazor.Validator;

public partial class PasswordAttribute:ValidationAttribute
{
    private static readonly Regex _regex = PasswordRegex();

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return ValidationResult.Success;
        }

        if (_regex.IsMatch(value.ToString()!))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(ErrorMessage);
    }

    [GeneratedRegex(@"^(?=.*[A-Z])(?=.*\d)(?=.*\W)[A-Za-z\d\W]{8,}$")]
    private static partial Regex PasswordRegex();

}