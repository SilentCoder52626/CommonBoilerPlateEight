using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CommonBoilerPlateEight.Domain.Constants;
using CommonBoilerPlateEight.Domain.Exceptions;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models;
using System.Data;
using X.PagedList;
using System.Security.Claims;
using CommonBoilerPlateEight.Domain.Helper;
using CommonBoilerPlateEight.Domain.Models.Permission;
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
        public async Task AssignPermission(string roleId, string permission)
        {

            using (var tx = TransactionScopeHelper.GetInstance())
            {
                var role = await _roleManager.FindByIdAsync(roleId).ConfigureAwait(false) ?? throw new CustomException("Role not found.");
                var rolePermissions = await _roleManager.GetClaimsAsync(role).ConfigureAwait(false);
                if (!rolePermissions.Any(x =>
                x.Type == Permission.PermissionClaimType &&
                         x.Value == permission))
                {
                    var roleClaim = await _roleManager.AddClaimAsync(role, new Claim(Permission.PermissionClaimType, permission)).ConfigureAwait(false);
                    if (!roleClaim.Succeeded) throw new CustomException("Error to Assign Permission");
                }
                tx.Complete();
            }

        }
        public async Task AssignAllPermissionOfModule(string roleId, string module)
        {

            using (var tx = TransactionScopeHelper.GetInstance())
            {
                var role = await _roleManager.FindByIdAsync(roleId).ConfigureAwait(false) ?? throw new CustomException("Role not found.");
                var allPermissionsOfModule = PermissionHelper.GetPermission().PermissionDictionary.Where(a => a.Key == module).SelectMany(p => p.Value.Select(i => $"{p.Key}-{i}"));
                var rolePermissions = await _roleManager.GetClaimsAsync(role).ConfigureAwait(false);

                foreach (var permission in allPermissionsOfModule)
                {
                    if (!rolePermissions.Any(x => x.Type == Permission.PermissionClaimType && x.Value == permission))
                    {
                        var roleClaim = await _roleManager.AddClaimAsync(role, new Claim(Permission.PermissionClaimType, permission)).ConfigureAwait(false);
                        if (!roleClaim.Succeeded) throw new CustomException("Error to Assign Permission");
                    }
                }
                tx.Complete();

            }

        }


        public async Task<PermissionDto> GetALLPermissions(string roleId)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(roleId) ?? throw new CustomException("Role not found.");
                var rolesPermission = await _roleManager.GetClaimsAsync(role).ConfigureAwait(false);
                var permissionDto = new PermissionDto() { RoleId = roleId };
                var AllPermissions = PermissionHelper.GetPermission().PermissionDictionary.OrderBy(a => a.Key);

                foreach (var permission in AllPermissions)
                {
                    var moduleWisePermission = new ModuleWisePermissionDto { Module = permission.Key };
                    foreach (var data in permission.Value.OrderBy(a => a))
                    {

                        moduleWisePermission.PermissionData.Add(new PermissionValues()
                        {
                            IsAssigned = rolesPermission.Any(x =>
                           x.Type == Permission.PermissionClaimType &&
                             x.Value == permission.Key + "-" + data),
                            Value = data
                        });
                    }
                    permissionDto.Permissions.Add(moduleWisePermission);
                }

                return permissionDto;

            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task UnAssignPermission(string roleId, string permission)
        {
            using (var tx = TransactionScopeHelper.GetInstance())
            {
                var role = await _roleManager.FindByIdAsync(roleId).ConfigureAwait(false) ?? throw new CustomException("Role not found.");
                var rolePermissions = await _roleManager.GetClaimsAsync(role).ConfigureAwait(false);
                if (rolePermissions.Any(x =>
                x.Type == Permission.PermissionClaimType &&
                         x.Value == permission))
                {
                    var claim = rolePermissions.Where(x =>
                x.Type == Permission.PermissionClaimType &&
                         x.Value == permission).First();

                    var response = await _roleManager.RemoveClaimAsync(role, claim);
                    if (!response.Succeeded) throw new CustomException("Error to UnAssign Permission");
                }
                tx.Complete();

            }
        }

        public async Task UnAssignPermissionOfModule(string roleId, string module)
        {

            using (var tx = TransactionScopeHelper.GetInstance())
            {
                var role = await _roleManager.FindByIdAsync(roleId).ConfigureAwait(false) ?? throw new CustomException("Role not found.");
                var allPermissionsOfModule = PermissionHelper.GetPermission().PermissionDictionary.Where(a => a.Key == module).SelectMany(p => p.Value.Select(i => $"{p.Key}-{i}"));
                var rolePermissions = await _roleManager.GetClaimsAsync(role).ConfigureAwait(false);

                foreach (var permission in allPermissionsOfModule)
                {
                    if (rolePermissions.Any(x => x.Type == Permission.PermissionClaimType && x.Value == permission))
                    {
                        var claim = rolePermissions.Where(x =>
                x.Type == Permission.PermissionClaimType &&
                         x.Value == permission).First();

                        var response = await _roleManager.RemoveClaimAsync(role, claim);

                        if (!response.Succeeded) throw new CustomException("Error to Assign Permission");
                    }
                }
                tx.Complete();
            }
        }
        public async Task AssignPermissionInBulk(string roleName, List<string> permissions)
        {

            using (var tx = TransactionScopeHelper.GetInstance())
            {
                var role = await _roleManager.FindByNameAsync(roleName).ConfigureAwait(false) ?? throw new CustomException("Role not found.");
                var rolePermissions = await _roleManager.GetClaimsAsync(role).ConfigureAwait(false);
                foreach (var permission in permissions)
                {
                    if (!rolePermissions.Any(x =>
                    x.Type == Permission.PermissionClaimType &&
                             x.Value == permission))
                    {
                        var roleClaim = await _roleManager.AddClaimAsync(role, new Claim(Permission.PermissionClaimType, permission)).ConfigureAwait(false);
                        if (!roleClaim.Succeeded) throw new CustomException("Error to Assign Permission");
                    }
                }
                tx.Complete();
            }

        }
    }
}
