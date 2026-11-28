using System.Globalization;
using CRM.Model.InputModels;
using MudBlazor;

namespace CRM.WebBlazor.Components.Pages.Identity;

public partial class ConfirmEmail
{
     private bool _success;
    private string[] _errors = Array.Empty<string>();
    private MudForm _form = new();
    private ApplicationUserConfirmEmailInputModel _model = new()
    {
        Email = "",
        EmailTemplate = ""
    };
    private bool _loading = false;
    private bool _insertCodeEnabled = false;

    private async Task ResetForm()
    {
        await _form.ResetAsync();
        await _form.ResetValidationAsync();
        _errors = Array.Empty<string>();
    }
    protected override async void OnInitialized()
    {
        
        try
        {
            _model.EmailTemplate = await FileService.ReadFileAsync($"wwwroot/templates/ConfirmEmailTemplate.{CultureInfo.CurrentUICulture.Name}.html");
        }
        catch (Exception ex)
        {
            _model.EmailTemplate = Localizer["email-template-raw-text"];
        }

        if (!string.IsNullOrWhiteSpace(RegisteredEmail))
        {
            _model.Email=RegisteredEmail;
            
        }
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender || !AutoSend || string.IsNullOrWhiteSpace(_model.Email))
        {
            return;
        }

        await SendVerificationCodeAsync();
        await InvokeAsync(StateHasChanged);
    }
    private async Task SendVerificationCodeAsync()
    {
        if (_loading)
        {
            return;
        }
        
        _loading = true;
        _errors = Array.Empty<string>();
        await InvokeAsync(StateHasChanged);

        try
        {
            var response = await AuthenticationService.ConfirmEmailAsync(_model);
            if (response.IsSuccess)
            {
                _insertCodeEnabled = true;
            }
            else
            {
                _errors = [response.Message ?? string.Empty];

            }
        }
        catch (HttpRequestException ex)
        {
            _errors = [ex.Message ?? string.Empty];
        }
        finally
        {
            _loading = false;
            await InvokeAsync(StateHasChanged);
        }
    }
    private async Task SubmitForm()
    {
        if (_loading)
            return;

        _loading = true;

        
        var response = _insertCodeEnabled
            ? await AuthenticationService.VerifyEmailCodeAsync(_model)
            : await AuthenticationService.ConfirmEmailAsync(_model); 

        _loading = false;
        if (response.IsSuccess)
        {
            if (_insertCodeEnabled)
            {
                NavManager.NavigateTo("/Identity/login");
            }
            else
            {
                _insertCodeEnabled = true;
            }
        }
        else
        {
            _errors = [response.Message??string.Empty];
        }
    }



}