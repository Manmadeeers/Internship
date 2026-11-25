using System.Text.Json;
using CRM.Model.ApplicationModels;
using CRM.Model.InputModels;
using CRM.Model.ViewModels;
using Microsoft.JSInterop;

namespace CRM.WebBlazor.Service.Identity;

internal sealed class AuthenticationService(IHttpClientFactory httpClientFactory, IErrorHandlingService errorHandlingService,IJSRuntime jsRuntime):IAuthenticationService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("DefaultClient");
    
    public async Task<ResponseModel<AuthenticationTokens >> LoginAsync(ApplicationUserLoginInputModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/Identity/Authentication/login", model);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<ResponseModel<AuthenticationTokens>>();
            if (result is { IsSuccess: true, Data: not null })
            {
                //TODO: Save tokens to localstorage
                return new ResponseModel<AuthenticationTokens>
                {
                    IsSuccess = true
                    
                };
            }
          
        }
        return await errorHandlingService.HandleErrorResponse<AuthenticationTokens >(response);
    }

    public async Task<ResponseModel<bool>> RegisterAsync(ApplicationUserRegisterInputModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/Identity/Authentication/register", model);
        if (response.IsSuccessStatusCode)
        {
            return new ResponseModel<bool>
            {
                IsSuccess = true
            };
        }
        return await errorHandlingService.HandleErrorResponse<bool>(response);
    }
    public async Task<ResponseModel<bool>> ConfirmEmailAsync(ApplicationUserConfirmEmailInputModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/Identity/Authentication/confirm-email", model);
        if (response.IsSuccessStatusCode)
        {
            return new ResponseModel<bool>
            {
                IsSuccess = true
            };
        }

        return await errorHandlingService.HandleErrorResponse<bool>(response);
    }

    public async Task<ResponseModel<bool>> VerifyEmailCodeAsync(ApplicationUserConfirmEmailInputModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/Identity/Authentication/confirm-email-verify-code", model);
        if (response.IsSuccessStatusCode)
        {
            return new ResponseModel<bool>
            {
                IsSuccess = true
            };
        }

        return await errorHandlingService.HandleErrorResponse<bool>(response);
    }
    
    public async Task<ResponseModel<bool>> ChangePasswordAsync(ApplicationUserForgotPasswordInputModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/Identity/Authentication/change-password", model);
        if (response.IsSuccessStatusCode)
        {
            return new ResponseModel<bool>
            {
                IsSuccess = true
            };
        }
        return await errorHandlingService.HandleErrorResponse<bool>(response);
    }

    public async Task<ResponseModel<bool>> ResetPasswordAsync(ApplicationUserResetPasswordInputModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/Identity/Authentication/reset-password", model);
        if (response.IsSuccessStatusCode)
        {
            return new ResponseModel<bool>
            {
                IsSuccess = true
            };
        }

        return await errorHandlingService.HandleErrorResponse<bool>(response);
    }

    public async Task<ResponseModel<bool>> ForgotPasswordAsync(ApplicationUserForgotPasswordInputModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/Identity/Authentication/forgot-password", model);
        if (response.IsSuccessStatusCode)
        {
            return new ResponseModel<bool>
            {
                IsSuccess = true
            };
        }
        
        return await errorHandlingService.HandleErrorResponse<bool>(response);
    }


    

}