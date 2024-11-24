using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Exceptions;
using CommonBoilerPlateEight.Domain.Extensions;
using CommonBoilerPlateEight.Domain.Helper;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models;
using X.PagedList;


namespace CommonBoilerPlateEight.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task BlockUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id).ConfigureAwait(false) ?? throw new CustomException("User not found");
            user.BlockUser();
            await _userManager.UpdateAsync(user).ConfigureAwait(false);
        }

        public async Task<string> Create(CreateUserViewModel dto)
        {
            using var tx = TransactionScopeHelper.GetInstance();
            await ValidateUser(dto.PhoneNumber, dto.UserName, dto.EmailAddress);
            var user = new ApplicationUser
            {
                FullName = dto.FullName,
                UserName = dto.UserName,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.EmailAddress,
                CreatedOn = DateTime.Now
            };
            var result = await _userManager.CreateAsync(user, dto.Password).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors = errors + $"{error.Description} <br>";
                }
                throw new CustomException(errors);
            }
            await AddRoleToUser(user, dto.RoleId);
            tx.Complete();
            return user.Id;

        }
        private async Task AddRoleToUser(ApplicationUser user, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId) ?? throw new CustomException("Role not found");
            await _userManager.AddToRoleAsync(user, role.Name);
        }

      
        public async Task Edit(UpdateUserViewModel dto)
        {
            using var tx = TransactionScopeHelper.GetInstance();
            var user = await _userManager.FindByIdAsync(dto.Id).ConfigureAwait(false) ?? throw new CustomException("User not found");
            await ValidateUser(dto.PhoneNumber, dto.UserName, dto.EmailAddress, user.Id);
            user.FullName = dto.FullName;
            user.UserName = dto.UserName;
            user.Email = dto.EmailAddress;
            user.PhoneNumber = dto.PhoneNumber;       
            var response = await _userManager.UpdateAsync(user).ConfigureAwait(false);
            if (!response.Succeeded) throw new CustomException(string.Join("<br>", response.Errors.Select(a => a.Description).ToList()));
            await UpdateRoleOfUser(dto.RoleId, user);
            tx.Complete();
        }

        public async Task<UserResponseViewModel> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id).ConfigureAwait(false) ?? throw new CustomException("User not found");
            var returnData =   new UserResponseViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                EmailAddress = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsBlocked = user.LockoutEnd >= DateTime.Now
            };
            var userRoles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            var role = await _roleManager.FindByNameAsync(userRoles.FirstOrDefault());
            returnData.RoleId = role.Id;
            return returnData;

        }
        private async Task UpdateRoleOfUser(string roleId, ApplicationUser? user)
        {
            var role = await _roleManager.FindByIdAsync(roleId).ConfigureAwait(false) ?? throw new CustomException("Role Not Found");
            var existingRoles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            if (existingRoles != null)
            {
                foreach (var userRole in existingRoles)
                {
                    await _userManager.RemoveFromRoleAsync(user, userRole);
                }
            }
            await _userManager.AddToRoleAsync(user, role.Name).ConfigureAwait(false);
        }
        private static UserResponseViewModel PopulateAdminUserResponseDto(ApplicationUser user)
        {
            return new UserResponseViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                EmailAddress = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsBlocked = user.LockoutEnd >= DateTime.Now
            };
        }

        public async Task<IPagedList<UserResponseViewModel>> GetAllAsPagedList(AdminUserFilterViewModel filter)
        {
            var userQueryable = _userManager.Users;
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                userQueryable = userQueryable.Where(a => a.PhoneNumber.ToLower().Equals(filter.Search.ToLower()) || a.Email.ToLower().Equals(filter.Search.ToLower()) || a.UserName.ToLower().Equals(filter.Search.ToLower()) || a.FullName.ToLower().Contains(filter.Search.ToLower()));
            }

            var userList = await userQueryable.Select(a => PopulateAdminUserResponseDto(a)).ToPagedListAsync(filter.PageNo, filter.PageSize);
            return userList;
        }

        public async Task UnblockUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id).ConfigureAwait(false) ?? throw new CustomException("User not found");
            user.UnBlockUser();
            await _userManager.UpdateAsync(user).ConfigureAwait(false);
        }

        public async Task ChangePassword(ChangePasswordViewModel model)
        {
            var userId = AppHttpContext.GetAdminCurrentUserId();
            var user = await _userManager.FindByIdAsync(userId) ?? throw new CustomException("User not found");
            var result = await _userManager.ChangePasswordAsync(user, model.OldPasword, model.NewPassword).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                throw new CustomException(string.Join("<br>", result.Errors.Select(a => a.Description).ToList()));
            }

        }

        public async Task ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId) ?? throw new CustomException("User not found");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
           
            if (!result.Succeeded)
            {
                throw new CustomException(string.Join("<br>", result.Errors.Select(a => a.Description).ToList()));
            }
        }

        private async Task ValidateUser(string phoneNumber, string email, string userName, string? userId = null)
        {

            var userWithSamePhoneNumber = await _userManager.Users.Where(a => a.PhoneNumber == phoneNumber).FirstOrDefaultAsync().ConfigureAwait(false);
            if (userWithSamePhoneNumber != null && userId != userWithSamePhoneNumber.Id) throw new CustomException("User With Same Number Already Exists");
            var userWithSameEmail = await _userManager.Users.Where(a => a.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync().ConfigureAwait(false);
            if (userWithSameEmail != null && userId != userWithSameEmail.Id) throw new CustomException("User With Same Email Already Exists");
            var userWithSameUserName = await _userManager.Users.Where(a => a.UserName.ToLower() == userName.ToLower()).FirstOrDefaultAsync().ConfigureAwait(false);
            if (userWithSameUserName != null && userId != userWithSameUserName.Id) throw new CustomException("User With Same Username Already Exists");
        }

       
    }
}