using CRM.Model.ApplicationModels;
using CRM.Model.IdentityModels;
using CRM.Model.InputModels;
using CRM.Model.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CRM.Service.Identity;

public class UserService(UserManager<ApplicationUser>userManager):IUserService
{
    public async Task<ResponseModel<ApplicationUserProfileViewModel>> GetUserProfileAsync(
        ApplicationUserContext context)
    {
        ArgumentNullException.ThrowIfNull(context.UserId);
        var user = await userManager.FindByIdAsync(context.UserId) ?? throw new Exception("User not found");
        return new ResponseModel<ApplicationUserProfileViewModel>
        {
            IsSuccess = true,
            Message = "User profile retrieved successfully",
            Data = new ApplicationUserProfileViewModel(user)
        };
    }
    public async Task<ResponseModel<bool>> UpdateUserProfileAsync(ApplicationUserProfileInputModel model, ApplicationUserContext context)
    {
       ArgumentNullException.ThrowIfNull(context.UserId);
       ArgumentNullException.ThrowIfNull(model.FirstName);
       ArgumentNullException.ThrowIfNull(model.LastName);
       
       var user = await userManager.FindByIdAsync(context.UserId)??throw new Exception("User not found");
       user.FirstName = model.FirstName;
       user.LastName = model.LastName;
       if (model.DateOfBirth.HasValue)
       {
           user.DateOfBirth = model.DateOfBirth.Value;
       }

       if (model.Gender.HasValue)
       {
           user.Gender = model.Gender.Value;
       }

       if (!string.IsNullOrEmpty(model.ImageName))
       {
           user.ImageName = model.ImageName;
       }
       var result = await userManager.UpdateAsync(user);

       return new ResponseModel<bool>
       {
           IsSuccess = result.Succeeded,
           Message = result.Succeeded ? "Profile successfully updated" : "Failed to update user profile"
       };

    }

    public async Task<ResponseModel<bool>> ChangePasswordAsync(ApplicationUserChangePasswordInputModel model, ApplicationUserContext context)
    {
        ArgumentNullException.ThrowIfNull(context.UserId);
        ArgumentNullException.ThrowIfNull(model.CurrentPassword);
        ArgumentNullException.ThrowIfNull(model.NewPassword);

        var user = await userManager.FindByIdAsync(context.UserId) ?? throw new Exception("User not found");

        var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        return new ResponseModel<bool>
        {
            IsSuccess = result.Succeeded,
            Message = result.Succeeded ? "Password successfully changed" : "Failed to change password"
        };
    } 
}