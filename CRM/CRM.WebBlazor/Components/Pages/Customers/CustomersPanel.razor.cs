using CRM.Model.ViewModels.Customers;
using MudBlazor;

namespace CRM.WebBlazor.Components.Pages.Customers;

public partial class CustomersPanel
{
    private IReadOnlyList<CustomerViewModel> _customers = Array.Empty<CustomerViewModel>();
    
    private async Task LoadCustomersAsync()
    {
        var response = await CustomersService.GetAllCustomersAsync();

        if (response is { IsSuccess: true, Data: not null })
        {
            _customers = response.Data;
        }
    }
    
    private async Task DeleteCustomerAsync(CustomerViewModel customer)
    {
        var response = await CustomersService.DeleteCustomerAsync(customer.Id);
        if (response)
        {
            await LoadCustomersAsync(); 
            Snackbar.Add(Localizer["message-customer-deleted"], Severity.Success);
        }
        else
        {
            Snackbar.Add(Localizer["message-unable-to-delete-customer"], Severity.Error);
            StateHasChanged();
        }
    }
    
    private static string GetInitials(CustomerViewModel customer)
    {
        var firstInitial = string.IsNullOrWhiteSpace(customer.FirstName)
            ? string.Empty
            : customer.FirstName[..1];

        var lastInitial = string.IsNullOrWhiteSpace(customer.LastName)
            ? string.Empty
            : customer.LastName[..1];

        return $"{firstInitial}{lastInitial}".ToUpperInvariant();
    }

    private static string FormatPhoneNumber(string phoneNumber)
    {
        return phoneNumber.StartsWith('+') ? phoneNumber : $"+{phoneNumber}";
    }
    
    
}