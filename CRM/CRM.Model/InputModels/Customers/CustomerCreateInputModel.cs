using System.ComponentModel.DataAnnotations;

namespace CRM.Model.InputModels.Customers;

public class CustomerCreateInputModel
{
    [MaxLength(100)]
    [Required]
    public required string FirstName { get; set; }
    [MaxLength(100)]
    [Required]
    public required string LastName { get; set; }
    [Phone]
    [MaxLength(30)]
    [Required]
    public required string PhoneNumber { get; set; }
    [EmailAddress]
    [MaxLength(256)]
    public  string? Email { get; set; }
    [MaxLength(256)]
    public string? CompanyName { get; set; }
}