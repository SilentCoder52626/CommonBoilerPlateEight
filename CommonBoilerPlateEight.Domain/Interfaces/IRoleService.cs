
using CommonBoilerPlateEight.Domain.Models;
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
    }
}
