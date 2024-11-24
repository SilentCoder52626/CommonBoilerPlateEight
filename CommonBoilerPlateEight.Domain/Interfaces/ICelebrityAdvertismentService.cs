using CommonBoilerPlateEight.Domain.Models;
using CommonBoilerPlateEight.Domain.Models.CelebrityAdvertisment;
using X.PagedList;

namespace CommonBoilerPlateEight.Domain.Interfaces
{
    public interface ICelebrityAdvertismentService
    {
        Task<CelebrityAdvertismentResponseModel> GetCelebrityAdvertismentAsync(string trackingId);
        Task<IPagedList<CelebrityAdvertismentResponseModel>> GetAdvertismentsOfACelebrityAsync(int celebrityId,
            CelebrityAdvertismentFilterViewModel model);
        Task<bool> AcceptAdvertisment(string trackingId);
        Task<bool> CancelAdvertisment(string trackingId, string comment);
        Task<bool> CompleteAdvertisment(string trackingId);
    }
}
