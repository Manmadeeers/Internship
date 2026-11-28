using CRM.Model.Companies;

namespace CRM.Model.ViewModels.Companies;

public class CompanyViewModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string WebSite { get; set; }
    public string?Email { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    
}