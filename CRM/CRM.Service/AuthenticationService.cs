using System.Net.Mail;
using CRM.Model.ApplicationModels;
using CRM.Model.IdentityModels;
using CRM.Model.InputModels;
using CRM.Service.IService;
using CRM.Utility.IUtility;
using Microsoft.AspNetCore.Identity;

namespace CRM.Service;
 
public class AuthenticationService(
    UserManager<ApplicationUser>userManager,
    SignInManager<ApplicationUser>signInManager,
    IApplicationEmailSender applicationEmailSender
    ): IAuthenticationService
{
    public Task<bool> ChangePasswordAsync(ApplicationUserRegisterInputModel model)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ForgotPasswordAsync(ApplicationUserRegisterInputModel model)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseModel<bool>> LoginAsync(ApplicationUserLoginInputModel model)
    {
        var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

        if (result.Succeeded)
        {
            return new ResponseModel<bool>
            {
                IsSuccess = true,
                Message = "Successfully logged in.",
                Data = true
            };
        }
        
        string errorMessage = result.IsLockedOut? "User locked out" : result.IsNotAllowed ? "Login is not allowed."
            : result.RequiresTwoFactor ? "Two-factor authentication is required" : "Invalid login attempt.";
        
        return new ResponseModel<bool>
        {
            IsSuccess = false,
            Message = errorMessage,
            Data = false
        };

    }

    public Task<bool> RefreshTokenAsync(ApplicationUserRegisterInputModel model)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseModel<bool>> RegisterAsync(ApplicationUserRegisterInputModel model)
    {
        ArgumentNullException.ThrowIfNull(model.Email);
        ArgumentNullException.ThrowIfNull(model.Password);

        var user = new ApplicationUser
        {
            Email = model.Email,
            UserName = model.Email,
            FirstName = model.FirstName!,
            LastName = model.LastName!,
            DateOfBirth = model.DateOfBirth,
            Gender = model.Gender,
            RegistrationDate = DateTime.UtcNow,
        };
        
        var result = await userManager.CreateAsync(user, model.Password);
        var errorMessage = result.Errors.Any()?string.Join("; ", result.Errors.Select(e=>e.Code)):"Unable to register user due to unknown errors";
        if (result.Succeeded)
        {
            return new ResponseModel<bool>
            {
                IsSuccess = true,
                Message = "Registration successfull",
                Data = true
            };
        }

        return new ResponseModel<bool>
        {
            IsSuccess = false,
            Message = "Unable to create user, Errors: "+errorMessage,
            Data = false
        };
    }

    public async Task<ResponseModel<bool>> ConfirmEmailAsync(ApplicationUserConfirmEmailInputModel model)
    {
        ArgumentNullException.ThrowIfNull(model.Email);
        
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return new ResponseModel<bool>
            {
                IsSuccess = false,
                Message = "User not found",
                Data = false
            };
        }

        if (user.EmailConfirmed)
        {
            return new ResponseModel<bool>
            {
                IsSuccess = false,
                Message = "Email is already confirmed",
                Data = false
            };
        }

        user.VerificationCode = GenerateVerificationCode();
        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return new ResponseModel<bool>
            {
                IsSuccess = false,
                Message = "Failed to save a verification code"
            };
        }


        model.Code = user.VerificationCode.ToString();
        model.FullName = $"{user.FirstName} {user.LastName}";
        
        await SendEmailConfirmationCodeAsync(model);

        return new ResponseModel<bool>
        {
            IsSuccess = true,
            Message = "Verification email sent"
        };

    }

    public async Task<ResponseModel<bool>> ConfirmEmailVerifyCodeAsync(ApplicationUserConfirmEmailInputModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user is null)
        {
            return new ResponseModel<bool>
            {
                IsSuccess = false,
                Message = "User not found",
                Data = false
            };
        }
        
        model.FullName = $"{user.FirstName} {user.LastName}";
        bool isCodeValid = user.VerificationCode!=null && user.VerificationCode .ToString() == model.Code;
        if (isCodeValid)
        {
            user.EmailConfirmed = true;
            user.Activity = true;
            var result = await userManager.UpdateAsync(user);
            return new ResponseModel<bool>
            {
                IsSuccess = true,
                Message = result.Succeeded?"Email confirmed successfully":"Email confirmation failed"
            };
        }
        else
        {
            return new ResponseModel<bool>
            {
                IsSuccess = false,
                Message = "Verification code is invalid"
            };
        }
         
    }
    

    public Task<bool> ResetPasswordAsync(ApplicationUserRegisterInputModel model)
    {
        throw new NotImplementedException();
    }

    private short GenerateVerificationCode()
    {
        Random random = new Random();
        return (short)random.Next(1000, 9999);
    }

    private async Task SendEmailConfirmationCodeAsync(ApplicationUserConfirmEmailInputModel model)
    {
        MailMessage mail = new MailMessage();
        mail.To.Add(new MailAddress(model.Email));
        mail.Subject = "Confirm your email";
        
        var emailContent = model.EmailTemplate.Replace("{FullName}",model.FullName).Replace("{Code}",model.Code);
        var alternateView = AlternateView.CreateAlternateViewFromString(emailContent, null, "text/html");
        mail.AlternateViews.Add(alternateView);
        await applicationEmailSender.SendEmailAsync(mail); 
    }
}  