using Business.Models;
using Data.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public interface IAuthService
    {
    Task<AuthResult> SignInAsync(SignInFormData formData);
    Task<AuthResult> SignOutAsync();
    Task<AuthResult> SignUpAsync(SignUpFormData formData);
    }

public class AuthService(IUserService userService, SignInManager<UserEntity> signInManager) : IAuthService
    {
    private readonly IUserService _userService = userService;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;

    public async Task<AuthResult> SignInAsync(SignInFormData formData)
        {
        if (formData == null)
            return new AuthResult
                {
                Success = false,
                Error = "Not all required fields are supplied.",
                StatusCode = 400
                };

        var result = await _signInManager.PasswordSignInAsync(formData.Email, formData.Password, formData.IsPersistent, lockoutOnFailure: false);
        return result.Succeeded
        ? new AuthResult
            {
            Success = true,
            StatusCode = 200,
            }
        : new AuthResult
            {
            Success = false,
            Error = "Invalid email or password.",
            StatusCode = 401
            };
        }
    public async Task<AuthResult> SignUpAsync(SignUpFormData formData)
        {
        if (formData == null)
            return new AuthResult
                {
                Success = false,
                Error = "Not all required fields are supplied.",
                StatusCode = 400
                };
        var result = await _userService.CreateUserAsync(formData);
        return result.Success
            ? new AuthResult
                {
                Success = true,
                StatusCode = 201,
                }
            : new AuthResult
                {
                Success = false,
                Error = result.Error,
                StatusCode = result.StatusCode,
                };
        }

    public async Task<AuthResult> SignOutAsync()
        {
        await _signInManager.SignOutAsync();
        return new AuthResult
            {
            Success = true,
            StatusCode = 200,
            };
        }
    }
