using CommonBoilerPlateEight.Domain.Models;
using X.PagedList;

namespace CommonBoilerPlateEight.Domain.Interfaces
{
    public interface IBookingService
    {
        Task CancelBookingByAdAsync(int adId, string reason);
        Task CheckoutAsync(int customerId);

        //Task<IPagedList<BookingDetailViewModel>> GetBookingsForCurrentCustomerAsync(int customerId, BookingFilterViewModel model);
        Task<BookingResponseModel> CreateBookingsFromAdminAsync(int customerId, List<CelebrityAdRequestModel> models);
        Task<IPagedList<AdvertisementDetailViewModel>> GetAdsForCurrentCustomerAsync(int customerId, AdvertisementFilterViewModel model);
        Task<AdvertisementDetailViewModel> GetAdvertisementByTrackingIdAsync(string trackingId);
        Task<CelebrityAdResponseModel> UpdateCelebrityAdvertisementFromAdminAsync(int adId, CelebrityAdRequestModel model);
        Task VerifyCheckoutAsync(int customerId);
    }
}
