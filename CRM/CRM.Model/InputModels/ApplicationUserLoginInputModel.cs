using System.ComponentModel.DataAnnotations;
using CRM.Model.Enums;

namespace CRM.Model.InputModels;

public class ApplicationUserLoginInputModel:ApplicationUserVerificationBaseInputModel
{
    
    [Required]
    public required string Password { get; set; }
}