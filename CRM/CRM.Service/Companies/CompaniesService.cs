using System.Net;
using CRM.DataAccess;
using CRM.Model.ApplicationModels;
using CRM.Model.Companies;
using CRM.Model.InputModels.Companies;
using CRM.Model.ViewModels.Companies;
using Microsoft.EntityFrameworkCore;

namespace CRM.Service.Companies;

public class CompaniesService(ApplicationDbContext context):ICompaniesService
{
    public async Task<ResponseModel<IReadOnlyList<CompanyModel>>> GetAllCompaniesAsync()
    {
        var companies = await context.Companies.AsNoTracking().ToListAsync();
        return new ResponseModel<IReadOnlyList<CompanyModel>>
        {
            IsSuccess = true,
            Message = "Companies loaded successfully",
            Data = companies
        };
    }

    public async Task<ResponseModel<CompanyModel>> GetCompanyByIdAsync(Guid id)
    {
        var company = await context.Companies.AsNoTracking().FirstOrDefaultAsync(c=>c.Id==id);
        if (company is null)
        {
            return new ResponseModel<CompanyModel>
            {
                IsSuccess = false,
                Message = "Company not found"
            };
        }

        return new ResponseModel<CompanyModel>
        {
            IsSuccess = true,
            Message = "Company loaded successfully",
            Data = company
        };
    }

    public async Task<ResponseModel<CompanyViewModel>> CreateCompanyAsync(CompanyCreateInputModel model)
    {
        ArgumentNullException.ThrowIfNull(model.Name);
        ArgumentNullException.ThrowIfNull(model.WebSite);
        
        if (await context.Companies.AnyAsync(c => c.Name == model.Name))
        {
            return new ResponseModel<CompanyViewModel>
            {
                IsSuccess = false,
                Message = "Company with this name already exists"
            };
        }

        var company = new CompanyModel
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            WebSite = model.WebSite
        };
        
        if (!string.IsNullOrWhiteSpace(model.Email))
        {
            if (await context.Companies.AnyAsync(c => c.Email == model.Email))
            {
                return new ResponseModel<CompanyViewModel>
                {
                    IsSuccess = false,
                    Message = "Company with this email already exists"
                };
            }
            
            company.Email = model.Email;
        }

        if (!string.IsNullOrWhiteSpace(model.Phone))
        {
            if (await context.Companies.AnyAsync(c => c.Phone == model.Phone))
            {
                return new ResponseModel<CompanyViewModel>
                {
                    IsSuccess = false,
                    Message = "Company with this phone number already exists"
                };
            }
            company.Phone = model.Phone;
        }

        if (!string.IsNullOrWhiteSpace(model.Address))
        {
            company.Address = model.Address;
        }
        
        await context.Companies.AddAsync(company);
        
        await context.SaveChangesAsync();
        return new ResponseModel<CompanyViewModel>
        {
            IsSuccess = true,
            Message = "Company added successfully",
            Data = new CompanyViewModel
            {
                Id = company.Id,
                Name = company.Name,
                WebSite = company.WebSite,
                Email = company.Email,
                Phone = company.Phone,
                Address = company.Address
            }
        };

    }

    public async Task<ResponseModel<HttpStatusCode>> UpdateCompanyAsync(Guid id, CompanyUpdateInputModel model)
    {
        var companyToUpdate = await context.Companies.FirstOrDefaultAsync(c => c.Id == id);
        if (companyToUpdate is null)
        {
            return new ResponseModel<HttpStatusCode>
            {
                IsSuccess = false,
                Message = "Company not found",
                Data = HttpStatusCode.NotFound
            };
        }

        if (!string.IsNullOrWhiteSpace(model.Name))
        {
            if (await context.Companies.AnyAsync(c => c.Name == model.Name))
            {
                return new ResponseModel<HttpStatusCode>
                {
                    IsSuccess = false,
                    Message = "Company with this name already exists",
                    Data = HttpStatusCode.Conflict
                };
            }
            companyToUpdate.Name = model.Name;
        }

        if (!string.IsNullOrWhiteSpace(model.WebSite))
        {
            if (await context.Companies.AnyAsync(c => c.WebSite == model.WebSite))
            {
                return new ResponseModel<HttpStatusCode>
                {
                    IsSuccess = false,
                    Message = "Company with this website already exists",
                    Data = HttpStatusCode.Conflict
                };
            }
            companyToUpdate.WebSite = model.WebSite;
        }

        if (!string.IsNullOrWhiteSpace(model.Phone))
        {
            if (await context.Companies.AnyAsync(c => c.Phone == model.Phone))
            {
                return new ResponseModel<HttpStatusCode>
                {
                    IsSuccess = false,
                    Message = "Company with this phone already exists",
                    Data = HttpStatusCode.Conflict
                };
            }
            companyToUpdate.Phone = model.Phone;
        }

        if (!string.IsNullOrWhiteSpace(model.Email))
        {
            if (await context.Companies.AnyAsync(c => c.Email == model.Email))
            {
                return new ResponseModel<HttpStatusCode>
                {
                    IsSuccess = false,
                    Message = "Company with this email already exists",
                    Data = HttpStatusCode.Conflict
                };
            }
            companyToUpdate.Email = model.Email;
        }

        if (!string.IsNullOrWhiteSpace(model.Address))
        {
            companyToUpdate.Address = model.Address;
        }
        
        await context.SaveChangesAsync();

        return new ResponseModel<HttpStatusCode>
        {
            IsSuccess = true,
            Message = "Company updated successfully",
            Data = HttpStatusCode.OK
        };
    }
    
    public async Task<ResponseModel<bool>> DeleteCompanyAsync(Guid id)
    {
        var companyToDelete = await context.Companies.FirstOrDefaultAsync(c => c.Id == id);
        if (companyToDelete is null)
        {
            return new ResponseModel<bool>
            {
                IsSuccess = false,
                Message = "Company not found"
            };
        }

        context.Companies.Remove(companyToDelete);

        await context.SaveChangesAsync();

        return new ResponseModel<bool>
        {
            IsSuccess = true,
            Message = "Company deleted successfully"
        };
    }
}