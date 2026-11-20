using System.ComponentModel.DataAnnotations;
using CRM.Model.Enums;

namespace CRM.Model.InputModels;

public class ApplicationUserRegisterInputModel:ApplicationUserVerificationBaseInputModel
{
    [Required]
    public string? FirstName { get; set; }
    
    [Required]
    public string? LastName { get; set; }
    
    public Gender? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? ImageName { get; set; }
    
    [Required]
    public required string Password { get; set; }
}