using System.ComponentModel.DataAnnotations;

namespace CRM.Model.InputModels.Companies;

public class CompanyUpdateInputModel
{
    [MaxLength(256)]
    public string?Name { get; set; }
    [MaxLength(256)]
    public string? WebSite { get; set; }
    [MaxLength(32)]
    public string? Phone { get; set; }
    [MaxLength(256)]
    public string? Email { get; set; }
    [MaxLength(256)]
    public string? Address { get; set; }
}