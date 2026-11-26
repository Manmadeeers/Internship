using CRM.Model.ApplicationModels;
using CRM.Model.InputModels;
using CRM.Model.ViewModels;

namespace CRM.WebBlazor.Service.Identity;

public class UserService(AuthenticatedHttpClientService httpClientService):IUserService
{
    public async Task<ResponseModel<ApplicationUserProfileViewModel>> GetUserProfileAsync()
    {
        return await httpClientService.GetAsync<ApplicationUserProfileViewModel>("/api/Identity/User/get-user-profile");
    }

    public async Task<ResponseModel<bool>> UpdateUserProfileAsync(ApplicationUserProfileInputModel model)
    {
        return await httpClientService.PutAsync<ApplicationUserProfileInputModel,bool>("/api/Identity/User/update-user-profile", model);
    }

    public async Task<ResponseModel<bool>> ChangePasswordAsync(ApplicationUserChangePasswordInputModel model)
    {
        return await httpClientService.PutAsync<ApplicationUserChangePasswordInputModel, bool>("/api/Identity/User/change-password", model);
    }
}