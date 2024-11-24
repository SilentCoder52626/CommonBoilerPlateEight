using Microsoft.AspNetCore.Http;
using CommonBoilerPlateEight.Domain.Models;
using CommonBoilerPlateEight.Domain.Models.Celebrity;
using X.PagedList;

namespace CommonBoilerPlateEight.Domain.Interfaces
{
    public interface ICelebrityService
    {
        Task<IPagedList<CelebrityBasicDetailViewModel>> GetAllAsPagedList(CelebrityFilterViewModel model);
        Task<List<DropdownDto>> GetCelebrityForDropdown();
        Task<GridResponseViewModel> GetFilteredCelebrity(int skip, int take, string? search);
        Task<int> CreateFromAdmin(CelebrityCreateViewModel viewModel);
        Task<CelebrityDetailResponseViewModel> GetById(int id);
        Task EditBasicDetail(CelebrityEditBasicDetailViewModel model);
        Task EditSocialLink(CelebritySocialLinkUpdateViewModel model);
        Task DeleteAttachment(int id);
        Task UploadAttachment(IFormFile file, string type, int celebrityId);
        Task Approve(int id);
        Task Reject(int id, string comment);
        Task ToogleConnectivity(int id);
        Task<string> GetCelebrityConnectivityStatus(int id);
        Task SetDeviceId(int id, string deviceId);
        Task Activate(int id);
        Task Deactivate(int id);
        Task<IPagedList<CelebrityBasicDetailViewModel>> GetRecommendedCelebrities(int customerId, int pageNumber, int pageSize);
        Task<IPagedList<CelebrityBasicDetailViewModel>> GetFilteredPageCelebrities(CelebrityFilterPageViewModel model, int CustomerId);
    }
}
