using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CommonBoilerPlateEight.Domain.Constants;
using CommonBoilerPlateEight.Domain.Exceptions;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models;
using System.Data;
using X.PagedList;
namespace CommonBoilerPlateEight.Domain.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<RoleViewModel> Create(CreateRoleViewModel model)
        {
            await ValidateRoleAsync(model.Name).ConfigureAwait(false);
            var identityRole = new IdentityRole(model.Name);
            var response = await _roleManager.CreateAsync(identityRole).ConfigureAwait(false);
            if (!response.Succeeded)
            {
                throw new CustomException(string.Join("</br>", response.Errors.Select(a => a.Description).ToList()));
            }
            var result = await _roleManager.FindByNameAsync(identityRole.Name);
            return new RoleViewModel()
            {
                Id = result.Id,
                Name = result.Name
            };
        }

        public async Task Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString()).ConfigureAwait(false) ?? throw new CustomException("Role not found.");
            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded) throw new CustomException(string.Join("</br>", result.Errors.Select(a => a.Description).ToList()));

        }

        public async Task<IPagedList<RoleViewModel>> GetAllAsPagedList(string search, int pageNumber = 1, int pageSize = 10)
        {
            var roleQueryable = _roleManager.Roles.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                roleQueryable = roleQueryable.Where(a => a.Name.ToLower().Contains(search.ToLower()));
            }
            var roles = await roleQueryable.Select(a => new RoleViewModel { Id = a.Id, Name = a.Name, IsEditable = a.Name.ToLower() == RoleConstant.RoleAdmin.ToLower() ? false : true }).ToPagedListAsync(pageNumber, pageSize);
            return roles;

        }

        public async Task<RoleViewModel> GetById(string id)
        {
            var role = await _roleManager.Roles.Where(a => a.Id == id).FirstOrDefaultAsync().ConfigureAwait(false) ?? throw new CustomException("Role not found.");
            return new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public async Task<List<RoleViewModel>> GetList()
        {
            var roles = await _roleManager.Roles.ToListAsync().ConfigureAwait(false);
            var roleResponses = roles
                .Select(role => new RoleViewModel
                {
                    Id = role.Id,
                    Name = role.Name
                })
                .ToList();
            return roleResponses;
        }

        public async Task Update(UpdateRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id).ConfigureAwait(false) ?? throw new CustomException("Role not found");
            await ValidateRoleAsync(model.Name, role).ConfigureAwait(false);
            role.Name = model.Name;
            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded) throw new CustomException(string.Join("</br>", result.Errors.Select(a => a.Description).ToList()));

        }

        private async Task ValidateRoleAsync(string roleName, IdentityRole? role = null)
        {
            var listOfUnAllowedRoles = new List<string>{
                RoleConstant.RoleAdmin.ToLower()
            };
            if (listOfUnAllowedRoles.Contains(roleName.ToLower().Trim())) throw new CustomException($"Duplicate role {roleName}.");
            var roleWithSameName = await _roleManager.FindByNameAsync(roleName).ConfigureAwait(false);
            if (roleWithSameName != null && roleWithSameName != role) throw new CustomException($"Duplicate role {roleName}.");
        }

    }
}
