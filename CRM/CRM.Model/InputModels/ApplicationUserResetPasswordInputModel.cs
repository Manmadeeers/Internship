using System.ComponentModel.DataAnnotations;

namespace CRM.Model.InputModels;

public class ApplicationUserResetPasswordInputModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }= string.Empty;
    
    [Required]
    public string Code { get; set; }= string.Empty;
    
    [Required]
    public string Password { get; set; }= string.Empty;
}