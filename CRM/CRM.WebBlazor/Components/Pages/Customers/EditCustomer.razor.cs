using CRM.Model.InputModels.Customers;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.WebBlazor.Components.Pages.Customers;

public partial class EditCustomer
{
    [Parameter]
    public Guid Id { get; set; }

    private MudForm _form = new();
    private CustomerUpdateInputModel _model = new();
    private bool _isValid;
    private bool _loading = true;
    private string[] _formErrors = Array.Empty<string>();
    private string[] _submitErrors = Array.Empty<string>();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        await LoadCustomerAsync();
    }

    private async Task LoadCustomerAsync()
    {
        try
        {
            var response = await CustomersService.GetCustomerByIdAsync(Id);

            if (!response.IsSuccess || response.Data is null)
            {
                Snackbar.Add(
                    response.Message ?? Localizer["message-unable-to-load-customer"],
                    Severity.Error);
                NavManager.NavigateTo("/customers/panel");
                return;
            }

            var customer = response.Data;
            _model = new CustomerUpdateInputModel
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                CompanyName = customer.CompanyName
            };
        }
        catch (HttpRequestException)
        {
            Snackbar.Add(Localizer["message-unable-to-load-customer"], Severity.Error);
            NavManager.NavigateTo("/customers/panel");
        }
        catch (TaskCanceledException)
        {
            Snackbar.Add(Localizer["message-unable-to-load-customer"], Severity.Error);
            NavManager.NavigateTo("/customers/panel");
        }
        finally
        {
            _loading = false;
            StateHasChanged();
        }
    }

    private async Task UpdateCustomerAsync()
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
            var response = await CustomersService.UpdateCustomerAsync(Id, _model);

            if (!response.IsSuccess)
            {
                _submitErrors =
                [response.Message ?? Localizer["message-unable-to-update-customer"]];
                return;
            }

            Snackbar.Add(Localizer["message-customer-updated"], Severity.Success);
            NavManager.NavigateTo("/customers/panel");
        }
        catch (HttpRequestException)
        {
            _submitErrors = [Localizer["message-unable-to-update-customer"]];
        }
        catch (TaskCanceledException)
        {
            _submitErrors = [Localizer["message-unable-to-update-customer"]];
        }
        finally
        {
            _loading = false;
        }
    }

    private void NormalizeModel()
    {
        _model.FirstName = _model.FirstName?.Trim();
        _model.LastName = _model.LastName?.Trim();
        _model.PhoneNumber = _model.PhoneNumber?.Trim();
        _model.Email = string.IsNullOrWhiteSpace(_model.Email)
            ? null
            : _model.Email.Trim().ToLowerInvariant();
        _model.CompanyName = string.IsNullOrWhiteSpace(_model.CompanyName)
            ? null
            : _model.CompanyName.Trim();
    }

    private string? ValidatePhoneNumber(string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return null;
        }

        var trimmedPhoneNumber = phoneNumber.Trim();
        var allowedCharacters = trimmedPhoneNumber.All(character =>
            char.IsDigit(character) || character is '+' or ' ' or '-' or '(' or ')');
        var digitCount = trimmedPhoneNumber.Count(char.IsDigit);
        var plusCount = trimmedPhoneNumber.Count(character => character == '+');
        var plusPlacementIsValid = plusCount == 0 ||
                                   plusCount == 1 && trimmedPhoneNumber.StartsWith('+');

        return allowedCharacters && plusPlacementIsValid && digitCount is >= 7 and <= 15
            ? null
            : Localizer["message-phone-format"];
    }

    private void GoBack()
    {
        NavManager.NavigateTo("/customers/panel");
    }
}
