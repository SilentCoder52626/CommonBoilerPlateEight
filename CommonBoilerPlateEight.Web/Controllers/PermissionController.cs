using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models.Permission;
using CommonBoilerPlateEight.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security;

namespace CommonBoilerPlateEight.Web.Controllers
{
    public class PermissionController : Controller
    {
        private readonly IRoleService _roleService;

        public PermissionController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        public async Task<IActionResult> Index(string RoleId)
        {
            try
            {
                var allPermissions = await _roleService.GetALLPermissions(RoleId).ConfigureAwait(true);
                var permissionViewModel = new PermissionViewModel { RoleId = RoleId };
                foreach (var permission in allPermissions.Permissions)
                {
                    var moduleWisePermission = new ModuleWisePermissionViewModel { Module = permission.Module };
                    foreach (var data in permission.PermissionData)
                    {
                        moduleWisePermission.PermissionData.Add(new PermissionValuesViewModel
                        {
                            IsAssigned = data.IsAssigned,
                            Value = data.Value
                        });
                    }
                    permissionViewModel.Permissions.Add(moduleWisePermission);
                }
                return View(permissionViewModel);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<IActionResult> LoadPermissionView(string RoleId)
        {
            try
            {
                var allPermissions = await _roleService.GetALLPermissions(RoleId).ConfigureAwait(true);
                var permissionViewModel = new PermissionViewModel { RoleId = RoleId };
                foreach (var permission in allPermissions.Permissions)
                {
                    var moduleWisePermission = new ModuleWisePermissionViewModel { Module = permission.Module };
                    foreach (var data in permission.PermissionData)
                    {
                        moduleWisePermission.PermissionData.Add(new PermissionValuesViewModel
                        {
                            IsAssigned = data.IsAssigned,
                            Value = data.Value
                        });
                    }
                    permissionViewModel.Permissions.Add(moduleWisePermission);
                }
                return PartialView("~/Areas/Account/Views/Permission/_AssignPermissionView.cshtml", permissionViewModel);
            }
            catch (Exception ex)
            {
                return this.ApiErrorResponse(HttpStatusCode.BadRequest, new List<string> { ex.Message }, Notify.Info.ToString());

            }
        }
        [HttpPost]
        public async Task<IActionResult> AssignPermission(string roleId, string permission)
        {
            try
            {
                await _roleService.AssignPermission(roleId, permission).ConfigureAwait(true);
                return this.ApiSuccessResponse(HttpStatusCode.OK, $"Permission {permission} assinged successfully");

            }
            catch (Exception ex)
            {
                return this.ApiErrorResponse(HttpStatusCode.BadRequest, new List<string> { ex.Message }, Notify.Info.ToString());
            }
        }
        public async Task<IActionResult> UnAssignPermission(string roleId, string permission)
        {
            try
            {
                await _roleService.UnAssignPermission(roleId, permission).ConfigureAwait(true);
                return this.ApiSuccessResponse(HttpStatusCode.OK, $"Permission {permission} unassinged successfully");

            }
            catch (Exception ex)
            {
                return this.ApiErrorResponse(HttpStatusCode.BadRequest, new List<string> { ex.Message }, Notify.Info.ToString());
            }
        }

        public async Task<IActionResult> AssignAllPermissionOfModule(string roleId, string module)
        {
            try
            {
                await _roleService.AssignAllPermissionOfModule(roleId, module).ConfigureAwait(true);
                return this.ApiSuccessResponse(HttpStatusCode.OK, $"Permission {module} assinged successfully");
            }
            catch (Exception ex)
            {
                return this.ApiErrorResponse(HttpStatusCode.BadRequest, new List<string> { ex.Message }, Notify.Info.ToString());
            }
        }
        public async Task<IActionResult> UnAssignAllPermissionOfModule(string roleId, string module)
        {
            try
            {
                await _roleService.UnAssignPermissionOfModule(roleId, module).ConfigureAwait(true);
                return this.ApiSuccessResponse(HttpStatusCode.OK, $"Module {module} unassinged successfully");

            }
            catch (Exception ex)
            {
                return this.ApiErrorResponse(HttpStatusCode.BadRequest, new List<string> { ex.Message }, Notify.Info.ToString());
            }
        }

    }
}
