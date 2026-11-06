using System.ComponentModel.DataAnnotations;
using CRM.Model.Enums;

namespace CRM.Model.InputModels;

public class ApplicationUserConfirmEmailInputModel
{
    [Required] [EmailAddress] public required string Email { get; set; }

    public required string EmailTemplate { get; set; }
    public string FullName { get; set; } = "CRM User";
    public string? Code { get; set; }
}