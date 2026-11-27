using CRM.Model.ApplicationModels;
using CRM.Model.ViewModels.Customers;

namespace CRM.WebBlazor.Service.Customers;

public class CustomersService(AuthenticatedHttpClientService httpClientService):ICustomersService
{
    public async Task<ResponseModel<IReadOnlyList<CustomerViewModel>>> GetAllCustomersAsync()
    {
        return await httpClientService.GetAsync<IReadOnlyList<CustomerViewModel>>("/api/Customers");
    }

    public async Task<bool> DeleteCustomerAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}