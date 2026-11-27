using System.Net;
using CRM.Model.ApplicationModels;
using CRM.Model.InputModels.Customers;
using CRM.Model.ViewModels.Customers;

namespace CRM.WebBlazor.Service.Customers;

public class CustomersService(AuthenticatedHttpClientService httpClientService):ICustomersService
{
    public async Task<ResponseModel<IReadOnlyList<CustomerViewModel>>> GetAllCustomersAsync()
    {
        return await httpClientService.GetAsync<IReadOnlyList<CustomerViewModel>>("/api/Customers");
    }

    public async Task<ResponseModel<CustomerViewModel>> GetCustomerByIdAsync(Guid id)
    {
        return await httpClientService.GetAsync<CustomerViewModel>($"/api/customers/{id}");
    }

    public async Task<ResponseModel<CustomerViewModel>> CreateCustomerAsync(CustomerCreateInputModel model)
    {
        return await httpClientService.PostAsync<CustomerCreateInputModel, CustomerViewModel>(
            "/api/Customers",
            model);
    }

    public async Task<ResponseModel<HttpStatusCode>> UpdateCustomerAsync(Guid id, CustomerUpdateInputModel model)
    {
        return await httpClientService.PutAsync<CustomerUpdateInputModel, HttpStatusCode>($"/api/customers/{id}",
            model);
    }

    public async Task<bool> DeleteCustomerAsync(Guid id)
    {
        var response = await httpClientService.DeleteAsync<bool>($"/api/customers/{id}");
        return response.IsSuccess;
    }
}
