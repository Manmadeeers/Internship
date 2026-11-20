using System.Globalization;
using CRM.Model.InputModels;
using MudBlazor;

namespace CRM.WebBlazor.Components.Pages.Identity;

public partial class ForgotPassword
{
    private bool _success; 
    private string[] _errors = Array.Empty<string>();
    private MudForm _form = new();
    private ApplicationUserForgotPasswordInputModel _forgotModel = new()  {
        Email = "",
        EmailTemplate = ""
    };
    private ApplicationUserResetPasswordInputModel _resetModel = new();
    private string? _confirmPassword;
    private bool _loading = false;
    private bool _insertCodeEnabled = false;
    
    protected override async void OnInitialized()
    {
        
        try
        {
            _forgotModel.EmailTemplate = await FileService.ReadFileAsync($"wwwroot/templates/ForgotPasswordTemplate.{CultureInfo.CurrentUICulture.Name}.html");
        }
        catch (Exception ex)
        {
            _forgotModel.EmailTemplate = Localizer["forgot-password-template-raw-text"];
        }
    }
    private async Task SubmitForm()
    {
        if (_loading)
        {
            return;
        }

        await _form.ValidateAsync();
        if (!_success)
        {
            return;
        }

        _loading = true;

        var response = _insertCodeEnabled
            ? await AuthenticationService.ResetPasswordAsync(_resetModel)
            : await AuthenticationService.ForgotPasswordAsync(_forgotModel); 

        _loading = false;
        if (response.IsSuccess)
        {
            if (_insertCodeEnabled)
            {
                NavManager.NavigateTo("/Identity/login");
            }
            else
            {
                _resetModel.Email = _forgotModel.Email;
                _insertCodeEnabled = true;
            }
        }
        else
        {
            _errors = [response.Message??string.Empty];
        }
    }
    private async Task ResetForm()
    {
        await _form.ResetAsync();
        await _form.ResetValidationAsync();
        _errors = Array.Empty<string>();
    }
    private string? PasswordMatch(string? confirmPassword)
    {
        return (_resetModel.Password == confirmPassword ? null : Localizer["message-confirm-password-match"])!;
    }






}