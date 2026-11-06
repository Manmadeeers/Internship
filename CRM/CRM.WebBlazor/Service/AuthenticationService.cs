using System.Net;
using CRM.Model.ApplicationModels;
using CRM.Model.InputModels;
using CRM.WebBlazor.LocalizationResource;
using Microsoft.Extensions.Localization;


namespace CRM.WebBlazor.Service;

internal sealed class AuthenticationService(HttpClient http, IStringLocalizer<Resource> localizer):IAuthenticationService
{

    public async Task<ResponseModel<bool>> ConfirmEmailAsync(ApplicationUserConfirmEmailInputModel model)
    {
        var response = await http.PostAsJsonAsync("/api/Identity/Authentication/confirm-email", model);
        if (response.IsSuccessStatusCode)
        {
            return new ResponseModel<bool>
            {
                IsSuccess = true
            };
        }

        return await HandleErrorResponse(response);
    }

    public async Task<ResponseModel<bool>> VerifyEmailCodeAsync(ApplicationUserConfirmEmailInputModel model)
    {
        var response = await http.PostAsJsonAsync("/api/Identity/Authentication/confirm-email-verify-code", model);
        if (response.IsSuccessStatusCode)
        {
            return new ResponseModel<bool>
            {
                IsSuccess = true
            };
        }

        return await HandleErrorResponse(response);
    }

    private async Task<ResponseModel<bool>> HandleErrorResponse(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<ResponseModel<bool>>();
            return new ResponseModel<bool>
            {
                IsSuccess = false,
                Message = GetLocalizedErrorMessage(errorResponse?.Message)?? localizer["message-unknown-error"]
            };
            
        }
        return new ResponseModel<bool>
        {
            IsSuccess = false,
            Message = localizer["message-unknown-error"]
        };

    }
    
    private string GetLocalizedErrorMessage(string? message)
    {
        var resourceKey = message switch
        {
            "User not found" => "message-user-not-found",
            "Email is already confirmed" => "message-email-already-confirmed",
            "Failed to save a verification code"=>"message-fail-to-save-verification-code",
            "Verification email sent"=>"message-verification-email-sent",
            "Email confirmed successfully"=>"message-email-confirmed-successfully",
            "Email confirmation failed"=>"message-email-confirmation-failed",
            "Verification code is invalid"=>"message-verification-code-invalid",
            _ => "message-unknown-error"
        };
        
        return localizer[resourceKey];
    } 
}