using CRM.Model.ApplicationModels;
using CRM.Model.Enums;
using CRM.Model.InputModels;
using CRM.Model.ViewModels;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace CRM.WebBlazor.Components.Pages.Identity;

public partial class UserProfile
{
    
    private bool _success;
    private string? _confirmPassword;
    private string[] _updateErrors = Array.Empty<string>();
    private string[] _passwordErrors = Array.Empty<string>();
    private MudForm _formUpdate = new();
    private MudForm _formChangePassword = new();
    private  ApplicationUserProfileViewModel _viewModel = new();
    private ApplicationUserProfileInputModel _updateModel = new() { FirstName = "", LastName = "" };
    private ApplicationUserChangePasswordInputModel _changePasswordModel = new() { CurrentPassword = "", NewPassword = "" };
    private bool _loading = false;
    private string _selectedGender = string.Empty;
    private const string ImageContainer = "user-profile-images";
    private const string DefaultAvatar = "avatar.png";

    private static string GetImagePath(string imageName) => $"../images/{ImageContainer}/{imageName}";
    private string _imageSrc = GetImagePath(DefaultAvatar);
    

    private async Task SubmitFormUpdate()
    {
        await _formUpdate.ValidateAsync();
        if (!_formUpdate.IsValid)
        {
            return;
        }
        UpdateGender(_selectedGender);
        
        _loading=true;
        var response = await UserService.UpdateUserProfileAsync(_updateModel);
        _loading = false;
        if (response.IsSuccess)
        {
            _viewModel.FirstName = _updateModel.FirstName;
            _viewModel.LastName = _updateModel.LastName;
            _viewModel.DateOfBirth =  _updateModel.DateOfBirth;
            _viewModel.ProfileDescription =  _updateModel.ProfileDescription;
            
            Snackbar.Add(Localizer["message-profile-updated-successfully"],Severity.Success);
        }
        else
        {
            _updateErrors = [response.Message??string.Empty];
        }
    }

    private async Task SubmitFormChangePassword()
    {
        await _formChangePassword.ValidateAsync();
        if (!_formChangePassword.IsValid)
        {
            return;
        }

        _loading = true;
        ResponseModel<bool> response = new();
        try
        {
          response = await UserService.ChangePasswordAsync(_changePasswordModel);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        _loading=false;

        if (response.IsSuccess)
        {
            Snackbar.Add(Localizer["message-password-changed-successfully"],Severity.Success);
        }
        else
        {
           Snackbar.Add(Localizer["message-password-changed-error"],Severity.Error);
        }
    }

    private KeyValuePair<Gender, string>[] GetLocalizedGenderArrayFromEnum()
    {
        return Enum.GetValues(typeof(Gender)).Cast<Gender>().Select(g => new KeyValuePair<Gender, string>(g, Localizer[$"gender-{g}"].Value)).ToArray();
    }
    
    private void UpdateGender(string value)
    {
        if (Enum.TryParse(value, out Gender gender))
        {
            _updateModel.Gender = gender;
        }
        else
        {
            _updateModel.Gender = null;
        }
    }

    private string? PasswordMatch(string value)
    {
        if (_changePasswordModel.NewPassword != _confirmPassword)
        {
            return Localizer["message-confirm-password-match"];
        }

        return null;
    }
    
    private async Task UploadImage(IBrowserFile file)
    {
        _loading=true;
        _updateModel.ImageName = await FileService.UploadImageAsync(file,ImageContainer);
        var response = await UserService.UpdateUserProfileAsync(_updateModel);
        _loading = false;

        if (response.IsSuccess)
        {
            _imageSrc = GetImagePath(_updateModel.ImageName);
            if (!string.IsNullOrEmpty(_updateModel.ImageName) && _viewModel.ImageName != DefaultAvatar)
            {
                await FileService.DeleteImageAsync(_viewModel.ImageName!,ImageContainer);
            }
            _viewModel.ImageName = _updateModel.ImageName;
        }
        else
        {
            Snackbar.Add(response.Message!, Severity.Error);
        }
    }



}