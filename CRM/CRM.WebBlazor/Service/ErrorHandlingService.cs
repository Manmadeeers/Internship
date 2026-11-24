using System.Net;
using CRM.Model.ApplicationModels;
using CRM.WebBlazor.LocalizationResource;
using Microsoft.Extensions.Localization;

namespace CRM.WebBlazor.Service;

internal sealed class ErrorHandlingService(IStringLocalizer<Resource> localizer):IErrorHandlingService
{
    public async Task<ResponseModel<T>> HandleErrorResponse<T>(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<ResponseModel<bool>>();
            return new ResponseModel<T>
            {
                IsSuccess = false,
                Message = GetLocalizedErrorMessage(errorResponse?.Message)
            };
            
        }
        return new ResponseModel<T>
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