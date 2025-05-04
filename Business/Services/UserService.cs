using Data.Entities;
using Data.Repositories;
using Domain.Extensions;
using Microsoft.AspNetCore.Identity;
using Business.Models;
using Domain.Models;
using System.Diagnostics;

namespace Business.Services;

public interface IUserService
    {
    Task<UserResult> AddUserToRoleAsync(string userId, string roleName);
    Task<UserResult> CreateUserAsync(SignUpFormData formData, string roleName = "User");
    Task<UserResult> GetUsersAsync();
    }

public class UserService(IUserRepository userRepository, UserManager<UserEntity> userManager, RoleManager<IdentityRole> roleManager) : IUserService
    {
    private readonly IUserRepository _userRepository = userRepository;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    public async Task<UserResult> GetUsersAsync()
        {
        var result = await _userRepository.GetAllAsync();
        return result.MapTo<UserResult>();
        }

    public async Task<UserResult> AddUserToRoleAsync(string userId, string roleName)
        {
        if (!await _roleManager.RoleExistsAsync(roleName))
            {
            return new UserResult
                {
                Success = false,
                Error = "Role does not exist",
                StatusCode = 404
                };
            }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            {
            return new UserResult
                {
                Success = false,
                Error = "User does not exist",
                StatusCode = 404
                };
            }
        var result = await _userManager.AddToRoleAsync(user, roleName);
        return result.Succeeded
            ? new UserResult
                {
                Success = true,
                StatusCode = 200,
                }
            : new UserResult
                {
                Success = false,
                Error = "Unable to add user to role",
                StatusCode = 500,
                };
        }
    public async Task<UserResult> CreateUserAsync(SignUpFormData formData, string roleName = "User")
        {
        if (formData == null)
            return new UserResult
                {
                Success = false,
                Error = "Form data can't be null",
                StatusCode = 400
                };

        var existResult = await _userRepository.ExistAsync(x => x.Email == formData.Email);
        if (existResult.Success)
            return new UserResult
                {
                Success = false,
                Error = "This email is already being used.",
                StatusCode = 409
                };
        try
            {

            var userEntity = formData.MapTo<UserEntity>();
            var result = await _userManager.CreateAsync(userEntity, formData.Password);

            if (result.Succeeded)
                {

                var addToRoleResult = await AddUserToRoleAsync(userEntity.Id, roleName);
                return result.Succeeded
                     ? new UserResult
                   {
                      Success = true,
                      StatusCode = 201,
                   }
                     : new UserResult
                   {
                      Success = false,
                      Error = "Unable to create user not added to role",
                      StatusCode = 201,
                   };
                }
                  return new UserResult
                {
                       Success = false,
                       Error = "Unable to create user.",
                       StatusCode = 201,
                };
            }


        catch (Exception ex)
            {
            Debug.WriteLine(ex.Message);
            return new UserResult
                {
                Success = false,
                Error = ex.Message,
                StatusCode = 500,
                };
            }
        }

    }