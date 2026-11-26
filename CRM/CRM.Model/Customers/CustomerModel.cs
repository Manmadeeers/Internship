using System.ComponentModel.DataAnnotations;

namespace CRM.Model.Customers;

public class CustomerModel
{
    public Guid Id { get; set; }
    [MaxLength(100)]
    public required string FirstName { get; set; }
    [MaxLength(100)]
    public required string LastName { get; set; }
    [MaxLength(256)]
    public string?Email { get; set; }
    public required int PhoneNumber { get; set; }
    [MaxLength(256)]
    public string?CompanyName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; } = [];
}