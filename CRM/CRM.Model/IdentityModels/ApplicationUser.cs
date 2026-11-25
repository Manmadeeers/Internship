using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM.Model.Enums;
using Microsoft.AspNetCore.Identity;

namespace CRM.Model.IdentityModels;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; } 
    public string? LastName { get; set; }
    public Gender? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime? RegistrationDate { get; set; }
    public short? VerificationCode { get; set; }
    public string? ImageName { get; set; }
    public bool? Activity { get; set; }
    public int? PhoneNumber { get; set; }
    
    [MaxLength(128)]
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public byte RefreshTokenAttemptCount { get; set; } = 0;

    [Timestamp] public byte[] RowVersion { get; set; } = [];
    [NotMapped] public string? FullName => $"{FirstName} {LastName}";
}