using CRM.Model.ApplicationModels;
using CRM.Model.InputModels.Customers;
using CRM.Model.ViewModels.Customers;

namespace CRM.Service.Customers;

public interface ICustomerService
{
    Task<ResponseModel<IReadOnlyList<CustomerViewModel>>> GetAllCustomersAsync();
    Task<ResponseModel<CustomerViewModel>>GetCustomerByIdAsync(int id);
    Task<ResponseModel<CustomerViewModel>> CreateCustomer(CustomerCreateInputModel model);
    Task<ResponseModel<bool>> UpdateCustomer(int id, CustomerUpdateInputModel model);
    Task<ResponseModel<bool>> DeleteCustomer(int id);
}