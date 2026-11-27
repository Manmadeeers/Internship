using CRM.Model.ApplicationModels;
using CRM.Model.ViewModels.Customers;

namespace CRM.WebBlazor.Service.Customers;

public interface ICustomersService
{
    Task<ResponseModel<IReadOnlyList<CustomerViewModel>>> GetAllCustomersAsync();
    Task<bool> DeleteCustomerAsync(Guid id);
}