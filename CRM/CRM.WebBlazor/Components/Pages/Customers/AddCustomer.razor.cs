using CRM.Model.InputModels.Customers;
using MudBlazor;

namespace CRM.WebBlazor.Components.Pages.Customers;

public  partial class AddCustomer
{
    private MudForm _form = new();
    private bool _isValid;
    private string[] _errors = Array.Empty<string>();
    private string[] _submitErrors = Array.Empty<string>();
    private CustomerCreateInputModel _model = new()
    {
        FirstName = string.Empty,
        LastName = string.Empty,
        PhoneNumber = string.Empty
    };
    
    
    private async Task CreateCustomerAsync()
    {
        await _form.ValidateAsync();

        if (!_form.IsValid || _loading)
        {
            return;
        }

        _loading = true;
        _submitErrors = Array.Empty<string>();

        try
        {
            NormalizeModel();
            var response = await CustomersService.CreateCustomerAsync(_model);

            if (!response.IsSuccess || response.Data is null)
            {
                _submitErrors =
                    [response.Message ?? Localizer["message-unable-to-create-customer"]];
                return;
            }

            Snackbar.Add(Localizer["message-customer-created"], Severity.Success);
            NavManager.NavigateTo("/customers/panel");
        }
        catch (HttpRequestException)
        {
            _submitErrors = [Localizer["message-unable-to-create-customer"]];
        }
        catch (TaskCanceledException)
        {
            _submitErrors = [Localizer["message-unable-to-create-customer"]];
        }
        finally
        {
            _loading = false;
        }
    }
    
    
    private void NormalizeModel()
    {
        _model.FirstName = _model.FirstName.Trim();
        _model.LastName = _model.LastName.Trim();
        _model.PhoneNumber = _model.PhoneNumber.Trim();
        _model.Email = string.IsNullOrWhiteSpace(_model.Email)
            ? null
            : _model.Email.Trim().ToLowerInvariant();
        _model.CompanyName = string.IsNullOrWhiteSpace(_model.CompanyName)
            ? null
            : _model.CompanyName.Trim();
    }

}