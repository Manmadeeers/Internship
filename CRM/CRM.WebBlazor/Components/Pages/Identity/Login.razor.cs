using System.Text.Json;
using CRM.Model.ApplicationModels;
using CRM.Model.InputModels;
using MudBlazor;

namespace CRM.WebBlazor.Components.Pages.Identity;

public partial class Login
{
    private bool _success;
    private string[] _errors = Array.Empty<string>();
    private MudForm _form = new();
    private ApplicationUserLoginInputModel _model = new() { Email = "", Password = "" };
    private bool _loading = false;

    private async Task HandleValidSubmit()
    {
        if (_loading)
            return;

        _loading = true;

        try
        {
            var response = await AuthenticationService.LoginAsync(_model);

            if (response.IsSuccess)
            {
                NavManager.NavigateTo("/api/Identity/user-profile");
                return;
            }
            
            _errors = [response.Message??string.Empty];
        }
        catch (HttpRequestException)
        {
            _errors = ["Unable to reach the login service."];
        }
        catch (TaskCanceledException)
        {
            _errors = ["The login request was canceled or timed out."];
        }
        finally
        {
            _loading = false;
        }
    }

    private void ChangeLoadingState()
    {
        _loading = !_loading;
    }
  
    private string GetLocalizedErrorMessage(string? message)
    {
        var resourceKey = GetResourceKeyForApiMessage(message);
        return Localizer[resourceKey];
    }

    private string GetResourceKeyForApiMessage(string? message)
    {
        return message switch
        {
            "User is locked out." => "message-user-locked-out",
            "Login is not allowed." => "message-login-not-allowed",
            "Two-factor authentication is required." => "message-two-factor-required",
            "Invalid login attempt." => "message-invalid-login",
            _ => "message-unknown-error"
        };
    }
}