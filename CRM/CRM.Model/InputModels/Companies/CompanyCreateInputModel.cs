using System.ComponentModel.DataAnnotations;

namespace CRM.Model.InputModels.Companies;

public class CompanyCreateInputModel
{
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
}