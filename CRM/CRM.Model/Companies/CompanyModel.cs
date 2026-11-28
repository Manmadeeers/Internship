using System.ComponentModel.DataAnnotations;
using CRM.Model.Customers;

namespace CRM.Model.Companies;

public class CompanyModel
{
    public Guid Id { get; set; }
    [MaxLength(256)]
    public required string Name { get; set; }
    [MaxLength(256)]
    public required string WebSite { get; set; }
    [MaxLength(32)]
    public string? Phone { get; set; }
    [MaxLength(256)]
    public string? Email { get; set; }
    [MaxLength(256)]
    public string? Address { get; set; }

    public ICollection<CustomerModel> Customers { get; set; } = [];
}