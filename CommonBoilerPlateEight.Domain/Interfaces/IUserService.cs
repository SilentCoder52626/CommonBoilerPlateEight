
using CommonBoilerPlateEight.Domain.Models;
using X.PagedList;

namespace CommonBoilerPlateEight.Domain.Interfaces
{
    public interface IUserService
    {
        Task<string> Create(CreateUserViewModel dto);
        Task<IPagedList<UserResponseViewModel>> GetAllAsPagedList(AdminUserFilterViewModel filter);
        Task<UserResponseViewModel> GetById(string id);
        Task Edit(UpdateUserViewModel dto);
        Task BlockUser(string id);
        Task UnblockUser(string id);
        Task ChangePassword(ChangePasswordViewModel model);
        Task ResetPassword(ResetPasswordViewModel model);
    }
}