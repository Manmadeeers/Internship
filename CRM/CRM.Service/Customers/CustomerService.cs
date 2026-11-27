using System.Net;
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

    public async Task<ResponseModel<CustomerViewModel>> CreateCustomer(CustomerCreateInputModel model)
    {
        ArgumentNullException.ThrowIfNull(model.FirstName);
        ArgumentNullException.ThrowIfNull(model.LastName);

        var normalizedEmail = string.IsNullOrWhiteSpace(model.Email) ? null : model.Email.ToLowerInvariant();
        if (normalizedEmail is not null)
        {
            if (await context.Customers.AnyAsync(c => c.Email == normalizedEmail))
            {
                return new ResponseModel<CustomerViewModel>
                {
                    IsSuccess = false,
                    Message = "Customer with this email already exists"
                };
            }
        }
        
        var phoneExists = await context.Customers.AnyAsync(c => c.PhoneNumber == model.PhoneNumber);
        if (phoneExists)
        {
            return new ResponseModel<CustomerViewModel>
            {
                IsSuccess = false,
                Message = "Customer with this phone number already exists"
            };
        }

        var customer = new CustomerModel
        {
            Id = Guid.NewGuid(),
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            CompanyName = model.CompanyName
        };

        context.Customers.Add(customer);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return new ResponseModel<CustomerViewModel>
            {
                IsSuccess = false,
                Message = "Database failed to add a new customer"
            };
        }
        return new ResponseModel<CustomerViewModel>
        {
            IsSuccess = true,
            Message = "Customer created successfully",
            Data = new CustomerViewModel
            {
                Id=customer.Id,
                FirstName=customer.FirstName,
                LastName=customer.LastName,
                Email=customer.Email,
                PhoneNumber=customer.PhoneNumber,
                CompanyName=customer.CompanyName
            }
        };

    }

    public async Task<ResponseModel<HttpStatusCode>> UpdateCustomer(Guid id, CustomerUpdateInputModel model)
    {
        var customerToUpdate = await context.Customers.FirstOrDefaultAsync(c => c.Id == id);
        if (customerToUpdate is null)
        {
            return new ResponseModel<HttpStatusCode>
            {
                IsSuccess = false,
                Message = "Customer not found",
                Data = HttpStatusCode.NotFound
            };
        }

        if (!string.IsNullOrEmpty(model.FirstName))
        {
            customerToUpdate.FirstName = model.FirstName;
        }

        if (!string.IsNullOrEmpty(model.LastName))
        {
            customerToUpdate.LastName = model.LastName;
        }

        if (!string.IsNullOrEmpty(model.Email))
        {
            if (await context.Customers.AnyAsync(c => c.Email == model.Email))
            {
                return new ResponseModel<HttpStatusCode>
                {
                    IsSuccess = false,
                    Message = "Customer with this email already exists",
                    Data = HttpStatusCode.Conflict
                };
            }
            customerToUpdate.Email = model.Email;
        }

        if (!string.IsNullOrEmpty(model.PhoneNumber))
        {
            if (await context.Customers.AnyAsync(c => c.PhoneNumber != null && c.PhoneNumber == model.PhoneNumber))
            {
                return new ResponseModel<HttpStatusCode>
                {
                    IsSuccess = false,
                    Message = "Customer with this phone number already exists",
                    Data = HttpStatusCode.Conflict
                };
            }
            customerToUpdate.PhoneNumber = model.PhoneNumber;
        }

        if (!string.IsNullOrEmpty(model.CompanyName))
        {
            customerToUpdate.CompanyName = model.CompanyName;
        }

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return new ResponseModel<HttpStatusCode>
            {
                IsSuccess = false,
                Message = "Database failed to update a customer",
                Data = HttpStatusCode.Conflict
            };
        }

        return new ResponseModel<HttpStatusCode>
        {
            IsSuccess = true,
            Message = "Customer updated successfully",
        };
    }

    public async  Task<ResponseModel<bool>> DeleteCustomer(Guid id)
    {
        var customerToDelete = await context.Customers.FirstOrDefaultAsync(c => c.Id == id);
        if (customerToDelete is null)
        {
            return new ResponseModel<bool>
            {
                IsSuccess = false,
                Message = "Customer not found"
            };
        }

        context.Customers.Remove(customerToDelete);

        await context.SaveChangesAsync();

        return new ResponseModel<bool>
        {
            IsSuccess = true,
            Message = "Customer deleted successfully"
        };

    }
}