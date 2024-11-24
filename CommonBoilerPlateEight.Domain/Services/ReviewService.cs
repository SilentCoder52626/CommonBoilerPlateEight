using Microsoft.EntityFrameworkCore;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Exceptions;
using CommonBoilerPlateEight.Domain.Extensions;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models;

namespace CommonBoilerPlateEight.Domain.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IDbContext _db;
        public ReviewService(IDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ReviewDetailViewModel>> GetAllReviewsAsync()
        {
            return await _db.CelebrityReviews
                .Select(review => new ReviewDetailViewModel
                {
                    Id = review.Id,
                    Rating = review.Rating,
                    ReviewText = review.ReviewText,
                    IsApprovedByAdmin = review.IsApprovedByAdmin
                })
                .ToListAsync();
        }

        public async Task<bool> ApproveReviewByIdAsync(int reviewId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(a => a.Id == AppHttpContext.GetAdminCurrentUserId()).ConfigureAwait(false) ?? throw new CustomException("User not found");

            var review = await _db.CelebrityReviews.FindAsync(reviewId);
            if (review == null)
            {
                return false; // Review not found
            }

            review.IsApprovedByAdmin = true;
            review.MarkAsApproved(user);
            _db.CelebrityReviews.Update(review);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<List<ReviewDetailViewModel>> GetAllReviewByCelebrityIdAsync(int celebrityId)
        {
            // Retrieve advertisements with reviews for the specified celebrity
            var reviewedAds = await _db.CelebrityAdvertisements
                .Where(ad => ad.CelebrityId == celebrityId && ad.CelebrityReviews.Any()) // Only ads with reviews
                .Include(ad => ad.Celebrity)
                .Include(ad => ad.Customer)
                .Include(ad => ad.CelebrityReviews)
                .Select(ad => new ReviewDetailViewModel
                {
                    CelebrityName = ad.Celebrity.FullName ?? string.Empty,
                    Rating = ad.CelebrityReviews.Average(review => (decimal?)review.Rating) ?? 0,
                    Reviews = ad.CelebrityReviews.Select(review => new ReviewResponseModel
                    {
                        CustomerName = ad.Customer.FullName ?? string.Empty,
                        AdId = review.AdId,
                        ReviewId = review.Id,
                        Rating = review.Rating,
                        ReviewText = review.ReviewText,
                    }).ToList()
                })
                .ToListAsync();

            // Calculate the average rating across all reviews for the celebrity
            decimal averageRating = reviewedAds.SelectMany(ad => ad.Reviews)
                                                .Average(review => (decimal?)review.Rating) ?? 0;

            // Add a single entry at the end of the list for the average rating
            reviewedAds.Add(new ReviewDetailViewModel
            {
                CelebrityName = reviewedAds.FirstOrDefault()?.CelebrityName ?? string.Empty,
                Rating = averageRating,
                Reviews = new List<ReviewResponseModel>()
            });

            return reviewedAds;
        }



        public async Task<ReviewResponseModel> AddReviewAsync(ReviewRequestModel model)
        {
            var advertisement = await _db.CelebrityAdvertisements
        .Include(ad => ad.CelebrityReviews)
        .FirstOrDefaultAsync(ad => ad.Id == model.AdId)
        ?? throw new CustomException("Advertisement not found.");

            // Check if this advertisement has already been reviewed by its customer
            if (advertisement.CelebrityReviews.Any(r => r.AdId == model.AdId)) throw new CustomException("Customer has already reviewed this advertisement.");

            // Create and add the review entity
            var review = new CelebrityReview
            {
                AdId = model.AdId,
                Rating = model.Rating,
                ReviewText = model.ReviewText,
                CreatedDate = DateTime.UtcNow
            };

            await _db.CelebrityReviews.AddAsync(review);
            await _db.SaveChangesAsync();

            // Return the review response model
            return new ReviewResponseModel
            {
                ReviewId = review.Id,
                AdId = review.AdId,
                Rating = review.Rating,
                ReviewText = review.ReviewText,
            };
        }



    }
}
