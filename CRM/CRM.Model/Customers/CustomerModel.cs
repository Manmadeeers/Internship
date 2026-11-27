using System.ComponentModel.DataAnnotations;
using CRM.Model.Companies;

namespace CRM.Model.Customers;

public class CustomerModel
{
    public Guid Id { get; set; }
    public Guid? CompanyId { get; set; }
    public CompanyModel? Company { get; set; }
    [MaxLength(100)]
    public required string FirstName { get; set; }
    [MaxLength(100)]
    public required string LastName { get; set; }
    [MaxLength(256)]
    public string?Email { get; set; }
    [MaxLength(32)]
    public string? PhoneNumber { get; set; }
    
    [MaxLength(256)]
    public string?CompanyName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; } = [];
}