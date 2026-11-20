
using CRM.Model.ApplicationModels;
using CRM.Model.Enums;
using CRM.Model.InputModels;
using MudBlazor;



namespace CRM.WebBlazor.Components.Pages.Identity;

public partial class Register()
{
    private string? _confirmPassword;
    private bool _success;
    private string[] _errors = Array.Empty<string>();
    private MudForm _form = new();
    private ApplicationUserRegisterInputModel _model = new()
    {
            FirstName = "",
            LastName = "",
            Email = "",
            Password = ""
        };
    private bool _loading = false;
    
    private async Task HandleValidSubmit()
    {
        await _form.ValidateAsync();
        
        if (_loading)
            return;

        _loading = true;

        try
        {
            using var response = await HttpClient.PostAsJsonAsync(
                "/api/Identity/Authentication/register", _model);

            if (response.IsSuccessStatusCode)
            {
                var email = Uri.EscapeDataString(_model.Email);
                NavManager.NavigateTo($"identity/confirm-email?email={email}&autoSend=true");
                return;
            }

            var body = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(body))
            {
                _errors =
                [
                    $"Registration failed: HTTP {(int)response.StatusCode} " +
                    $"({response.ReasonPhrase}). The server returned an empty body."
                ];
                return;
            }

            try
            {
                var errorResponse =
                    System.Text.Json.JsonSerializer.Deserialize<ResponseModel<bool>>(
                        body,
                        new System.Text.Json.JsonSerializerOptions(
                            System.Text.Json.JsonSerializerDefaults.Web));

                _errors =
                [
                    GetLocalizedErrorMessage(errorResponse?.Message)
                ];
            }
            catch (System.Text.Json.JsonException)
            {
                _errors =
                [
                    $"Registration failed: HTTP {(int)response.StatusCode}. " +
                    "The server returned an unexpected response format."
                ];
            }
        }
        catch (HttpRequestException)
        {
            _errors = ["Unable to reach the registration service."];
        }
        catch (TaskCanceledException)
        {
            _errors = ["The registration request was canceled or timed out."];
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
    private string? PasswordMatch(string? confirmPassword)
    {
        return (_model.Password == confirmPassword ? null : Localizer["message-confirm-password-match"])!;
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
            "DuplicateUserName" => "message-email-exists",
            _ => "message-unknown-error"
        };
    }
    private KeyValuePair<Gender, string>[] GetLocalizedGenderArrayFromEnum()
    {
        return Enum.GetValues(typeof(Gender))
                   .Cast<Gender>()
                   .Select(gender => new KeyValuePair<Gender, string>(gender, Localizer[$"gender-{gender}"].Value))
                   .ToArray();
    }
    private void UpdateGender(string value)
    {
        if (Enum.TryParse<Gender>(value, out var parsedGender))
            _model.Gender = parsedGender;
        else
            _model.Gender = null;
    }
}