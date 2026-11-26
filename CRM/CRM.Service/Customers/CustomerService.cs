using CRM.DataAccess;
using CRM.Model.ApplicationModels;
using CRM.Model.Customers;
using CRM.Model.InputModels.Customers;
using CRM.Model.ViewModels.Customers;
using Microsoft.EntityFrameworkCore;

namespace CRM.Service.Customers;

public class CustomerService(ApplicationDbContext context):ICustomerService
{
    public async Task<ResponseModel<IReadOnlyList<CustomerModel>>> GetAllCustomersAsync()
    {
        var customers = await context.Customers.ToListAsync();
        return new ResponseModel<IReadOnlyList<CustomerModel>>
        {
            IsSuccess = true,
            Message = "Customers loaded successfully",
            Data = customers
        };
    }

    public async Task<ResponseModel<CustomerModel>> GetCustomerByIdAsync(Guid id)
    {
        var customer = await context.Customers.FirstOrDefaultAsync(c => c.Id == id);
        if (customer is null)
        {
            return new ResponseModel<CustomerModel>
            {
                IsSuccess = false,
                Message = "Customer not found"
            };
        }

        return new ResponseModel<CustomerModel>
        {
            IsSuccess = true,
            Message = "Customer loaded successfully",
            Data = customer
        };
    }

    public Task<ResponseModel<CustomerViewModel>> CreateCustomer(CustomerCreateInputModel model)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel<bool>> UpdateCustomer(Guid id, CustomerUpdateInputModel model)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel<bool>> DeleteCustomer(Guid id)
    {
        throw new NotImplementedException();
    }
}