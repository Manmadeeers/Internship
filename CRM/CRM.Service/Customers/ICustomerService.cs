using System.Net;
using CRM.Model.ApplicationModels;
using CRM.Model.Customers;
using CRM.Model.InputModels.Customers;
using CRM.Model.ViewModels.Customers;

namespace CRM.Service.Customers;

public interface ICustomerService
{
    Task<ResponseModel<IReadOnlyList<CustomerModel>>> GetAllCustomersAsync();
    Task<ResponseModel<CustomerModel>>GetCustomerByIdAsync(Guid id);
    Task<ResponseModel<CustomerViewModel>> CreateCustomer(CustomerCreateInputModel model);
    Task<ResponseModel<HttpStatusCode>> UpdateCustomer(Guid id, CustomerUpdateInputModel model);
    Task<ResponseModel<bool>> DeleteCustomer(Guid id);
}