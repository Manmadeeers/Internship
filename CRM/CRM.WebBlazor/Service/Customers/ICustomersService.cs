using System.Net;
using CRM.Model.ApplicationModels;
using CRM.Model.InputModels.Customers;
using CRM.Model.ViewModels.Customers;

namespace CRM.WebBlazor.Service.Customers;

public interface ICustomersService
{
    Task<ResponseModel<IReadOnlyList<CustomerViewModel>>> GetAllCustomersAsync();
    Task<ResponseModel<CustomerViewModel>> GetCustomerByIdAsync(Guid id);
    Task<ResponseModel<CustomerViewModel>> CreateCustomerAsync(CustomerCreateInputModel model);
    Task<ResponseModel<HttpStatusCode>> UpdateCustomerAsync(Guid id, CustomerUpdateInputModel model);
    Task<bool> DeleteCustomerAsync(Guid id);
}
