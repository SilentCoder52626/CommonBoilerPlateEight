
using CommonBoilerPlateEight.Domain.Models;
using CommonBoilerPlateEight.Domain.Models.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace CommonBoilerPlateEight.Domain.Interfaces
{
    public interface IRoleService
    {
        Task<IPagedList<RoleViewModel>> GetAllAsPagedList(string search,int pageNumber = 1, int pageSize = 10);
        Task<RoleViewModel> GetById(string id);
        Task<RoleViewModel> Create(CreateRoleViewModel model);
        Task Update(UpdateRoleViewModel model);
        Task Delete(string id);
        Task<List<RoleViewModel>> GetList();

        Task<PermissionDto> GetALLPermissions(string roleId);
        Task AssignPermission(string roleId, string permission);
        Task UnAssignPermission(string roleId, string permission);
        Task AssignAllPermissionOfModule(string roleId, string module);
        Task UnAssignPermissionOfModule(string roleId, string module);
        Task AssignPermissionInBulk(string roleName, List<string> permissions);
    }
}
