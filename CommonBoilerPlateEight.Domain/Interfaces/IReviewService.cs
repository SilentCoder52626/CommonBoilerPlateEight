using CommonBoilerPlateEight.Domain.Models;

namespace CommonBoilerPlateEight.Domain.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewResponseModel> AddReviewAsync(ReviewRequestModel model);
        Task<bool> ApproveReviewByIdAsync(int reviewId);
        Task<List<ReviewDetailViewModel>> GetAllReviewByCelebrityIdAsync(int celebrityId);
        Task<IEnumerable<ReviewDetailViewModel>> GetAllReviewsAsync();
    }
}
