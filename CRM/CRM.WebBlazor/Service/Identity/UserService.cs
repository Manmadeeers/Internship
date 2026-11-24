using System.Text.Json;
using CRM.Model.ApplicationModels;
using CRM.Model.InputModels;
using CRM.Model.ViewModels;
using Microsoft.JSInterop;

namespace CRM.WebBlazor.Service;

public class UserService(HttpClient httpClient, IJSRuntime jsRuntime, IErrorHandlingService errorHandlingService):IUserService
{
    public async Task<ApplicationUserProfileViewModel> GetUserProfileAsync()
    {
        var userLocalStorage = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "user");
        if (!string.IsNullOrEmpty(userLocalStorage))
        {
            var user = JsonSerializer.Deserialize<ApplicationUserProfileViewModel>(userLocalStorage);
            if (user != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.Token);
                return user;
            }
        }

        return new ApplicationUserProfileViewModel();
    }

    public async Task<ResponseModel<bool>> UpdateUserProfileAsync(ApplicationUserProfileInputModel model)
    {
        var response = await httpClient.PostAsJsonAsync("/api/Identity/User/update-user-profile", model);
        if (response.IsSuccessStatusCode)
        {
            return new ResponseModel<bool>
            {
                IsSuccess = true
            };
        }
        return await errorHandlingService.HandleErrorResponse<bool>(response);
    }

    public async Task<ResponseModel<bool>> ChangePasswordAsync(ApplicationUserChangePasswordInputModel model)
    {
        var response = await httpClient.PostAsJsonAsync("/api/Identity/User/change-password", model);
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