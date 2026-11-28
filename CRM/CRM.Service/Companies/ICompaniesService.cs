using System.Net;
using CRM.Model.ApplicationModels;
using CRM.Model.Companies;
using CRM.Model.InputModels.Companies;
using CRM.Model.InputModels.Customers;
using CRM.Model.ViewModels.Companies;

namespace CRM.Service.Companies;

public interface ICompaniesService
{
    Task<ResponseModel<IReadOnlyList<CompanyModel>>> GetAllCompaniesAsync();
    Task<ResponseModel<CompanyModel>> GetCompanyByIdAsync(Guid id);
    Task<ResponseModel<CompanyViewModel>> CreateCompanyAsync(CompanyCreateInputModel model);
    Task<ResponseModel<HttpStatusCode>>UpdateCompanyAsync(Guid id, CompanyUpdateInputModel model);
    Task<ResponseModel<bool>> DeleteCompanyAsync(Guid id);
}