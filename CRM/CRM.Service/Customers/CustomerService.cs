using CRM.Model.ApplicationModels;
using CRM.Model.InputModels.Customers;
using CRM.Model.ViewModels.Customers;

namespace CRM.Service.Customers;

public class CustomerService:ICustomerService
{
    public Task<ResponseModel<IReadOnlyList<CustomerViewModel>>> GetAllCustomersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel<CustomerViewModel>> GetCustomerByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel<CustomerViewModel>> CreateCustomer(CustomerCreateInputModel model)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel<bool>> UpdateCustomer(int id, CustomerUpdateInputModel model)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel<bool>> DeleteCustomer(int id)
    {
        throw new NotImplementedException();
    }
}