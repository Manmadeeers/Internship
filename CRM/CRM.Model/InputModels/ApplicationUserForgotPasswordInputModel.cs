using System.ComponentModel.DataAnnotations;

namespace CRM.Model.InputModels;

public class ApplicationUserForgotPasswordInputModel:ApplicationUserVerificationBaseInputModel
{
    [Required]
    public  string? Password { get; set; }
    
}