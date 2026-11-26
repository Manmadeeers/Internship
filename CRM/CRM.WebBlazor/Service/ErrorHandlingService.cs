using System.Net;
using CRM.Model.ApplicationModels;
using CRM.WebBlazor.LocalizationResource;
using Microsoft.Extensions.Localization;

namespace CRM.WebBlazor.Service;



internal sealed class ErrorHandlingService(IStringLocalizer<Resource> localizer,ILogger<ErrorHandlingService> logger):IErrorHandlingService
{
    public async Task<ResponseModel<T>> HandleErrorResponse<T>(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
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

    public ResponseModel<T> HandleErrorResponse<T>(ResponseModel<T>? response)
    {
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
            var value when value?.Contains("DuplicateUserName",StringComparison.OrdinalIgnoreCase)==true=>"message-name-exists",
            var value when value?.Contains("DuplicateEmail",StringComparison.OrdinalIgnoreCase)==true=>"message-email-exists",
            var value when value?.Contains("PasswordMismatch",StringComparison.OrdinalIgnoreCase)==true=>"message-password-mismatch",
            "User not found" => "message-user-not-found",
            "Email is already confirmed" => "message-email-already-confirmed",
            "Failed to save a verification code"=>"message-fail-to-save-verification-code",
            "Verification email sent"=>"message-verification-email-sent",
            "Email confirmed successfully"=>"message-email-confirmed-successfully",
            "Email confirmation failed"=>"message-email-confirmation-failed",
            "Verification code is invalid"=>"message-verification-code-invalid",
            "Invalid login attempt."=>"message-invalid-login",
            "Unable to create user"=>"message-unable-to-create-user",
            _ => "message-unknown-error"
        };

        if (resourceKey is "message-unknown-error")
        {
            logger.LogWarning(
                "Unmapped API error message: {ApiErrorMessage}",
                message ?? "<null>");
        }
        
        return localizer[resourceKey];
    } 

}