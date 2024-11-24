using Microsoft.EntityFrameworkCore;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Enums;
using CommonBoilerPlateEight.Domain.Exceptions;
using CommonBoilerPlateEight.Domain.Helper;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models;
using X.PagedList;

namespace CommonBoilerPlateEight.Domain.Services
{
    public class BookingService : IBookingService
    {
        private readonly IDbContext _db;
        private readonly ICartItemService _cartItemService;
        private readonly IWalletService _walletService;
        public BookingService(IDbContext db, ICartItemService cartItemService, IWalletService walletService)
        {
            _db = db;
            _cartItemService = cartItemService;
            _walletService = walletService;
        }

        public async Task VerifyCheckoutAsync(int customerId)
        {
            var cartItems = await GetCartItemsByCustomerIdAsync(customerId);

            foreach (var cartItem in cartItems)
            {
                if (cartItem.AdDate < DateTime.UtcNow)
                {
                    throw new CustomException($"The schedule for cart item with ID {cartItem.Id} has expired. Please update or remove this item before checkout.");
                }
            }
        }

        public async Task CheckoutAsync(int customerId)
        {
            using (var transaction = TransactionScopeHelper.GetInstance())
            {
                await VerifyCheckoutAsync(customerId);

                var totalAmount = await _cartItemService.CalculateCartTotalAsync(customerId);

                await _walletService.DeductFromWallet(customerId, totalAmount);

                var booking = await CreateBookingAsync(customerId);

                var cartItems = await GetCartItemsByCustomerIdAsync(customerId);

                await TransferToCelebrityAdsWithQuestionsAsync(booking.Id, cartItems);

                await ClearCustomerCartItemsAsync(customerId);

                transaction.Complete();
            }
        }

        private async Task<Booking> CreateBookingAsync(int customerId)
        {
            var booking = new Booking
            {
                CustomerId = customerId,
                CreatedDate = DateTime.UtcNow,
                Status = BookingStatusEnum.Pending,
                TrackingId = RandomStringGenerator.GenerateTrackingId("STRBO"),
            };
            _db.Bookings.Add(booking);
            await _db.SaveChangesAsync();

            AddBookingHistory(booking, "Booking created", BookingStatusEnum.Pending);
            return booking;
        }

        private async Task<List<CartItem>> GetCartItemsByCustomerIdAsync(int customerId)
        {
            return await _db.CartItems
                .Include(ci => ci.CartItemQuestions)
                .Where(ci => ci.CustomerId == customerId)
                .ToListAsync();
        }

        private async Task TransferToCelebrityAdsWithQuestionsAsync(int bookingId, List<CartItem> cartItems)
        {
            var adsWithQuestions = cartItems.Select(cartItem => new CelebrityAdvertisement
            {
                TrackingId = RandomStringGenerator.GenerateTrackingId("STRAD"),
                BookingId = bookingId,
                CustomerId = cartItem.CustomerId,
                CelebrityId = cartItem.CelebrityId,
                CelebrityScheduleId = cartItem.CelebrityScheduleId,
                CompanyTypeId = cartItem.CompanyTypeId,
                CountryId = cartItem.CountryId,
                CompanyName = cartItem.CompanyName,
                ContactPerson = cartItem.ContactPerson,
                ManagerPhone = cartItem.ManagerPhone,
                AdDate = cartItem.AdDate,
                AdPrice = cartItem.AdPrice,
                DeliveryType = cartItem.DeliveryType,
                Status = BookingStatusEnum.Pending,

                CelebrityAdvertismentQuestions = cartItem.CartItemQuestions.Select(q => new CelebrityAdvertismentQuestion
                {
                    QuestionId = q.QuestionId,
                    TextAnswer = q.TextAnswer,
                    SelectedOptionId = q.SelectedOptionId,
                    DateAnswer = q.DateAnswer,
                    NumberAnswer = q.NumberAnswer
                }).ToList()
            }).ToList();

            adsWithQuestions.ForEach(ad => AddAdvertismentHistory(ad, "Advertisement created.", BookingStatusEnum.Pending));

            await _db.CelebrityAdvertisements.AddRangeAsync(adsWithQuestions);
            await _db.SaveChangesAsync();
        }



        private async Task ClearCustomerCartItemsAsync(int customerId)
        {
            var cartItems = await _db.CartItems
                .Where(ci => ci.CustomerId == customerId)
                 .Include(ci => ci.CartItemQuestions)
                .ToListAsync();

            var cartItemQuestions = cartItems.SelectMany(ci => ci.CartItemQuestions).ToList();
            _db.CartItemQuestions.RemoveRange(cartItemQuestions);
            _db.CartItems.RemoveRange(cartItems);
            await _db.SaveChangesAsync();
        }


        public async Task<AdvertisementDetailViewModel> GetAdvertisementByTrackingIdAsync(string trackingId)
        {
            var advertisement = await _db.CelebrityAdvertisements
                .AsNoTracking()
                .Where(ad => ad.TrackingId == trackingId)
                .Select(ad => new AdvertisementDetailViewModel
                {
                    AdvertisementId = ad.Id,
                    TrackingId = ad.TrackingId,
                    CelebrityName = ad.Celebrity.FullName,
                    CompanyName = ad.CompanyName,
                    ContactPerson = ad.ContactPerson,
                    ManagerPhone = ad.ManagerPhone,
                    DeliveryType = ad.DeliveryType.ToString(),
                    Status = ad.Status.ToString(),
                    AdDate = ad.AdDate,
                    AdPrice = ad.AdPrice,
                })
                .FirstOrDefaultAsync()
                .ConfigureAwait(false)
                ?? throw new CustomException("Advertisement not found.");

            return advertisement;
        }

        public async Task<IPagedList<AdvertisementDetailViewModel>> GetAdsForCurrentCustomerAsync(int customerId, AdvertisementFilterViewModel model)
        {
            var adQueryable = _db.CelebrityAdvertisements
                .AsNoTracking()
                .Where(ad => ad.CustomerId == customerId);

            if (model.Status.HasValue)
            {
                adQueryable = adQueryable.Where(ad => ad.Status == model.Status.Value);
            }

            var advertisements = await adQueryable
                .Select(ad => new AdvertisementDetailViewModel
                {
                    AdvertisementId = ad.Id,
                    TrackingId = ad.TrackingId,
                    CelebrityName = ad.Celebrity.FullName ?? string.Empty,
                    Status = ad.Status.ToString(),
                    CompanyName = ad.CompanyName,
                    ContactPerson = ad.ContactPerson,
                    ManagerPhone = ad.ManagerPhone,
                    AdDate = ad.AdDate,
                    AdPrice = ad.AdPrice,
                    DeliveryType = ad.DeliveryType.ToString(),
                })
                .ToPagedListAsync(model.PageNumber, model.pageSize)
                .ConfigureAwait(false);

            return advertisements;
        }

        //Changes will be made in below methods in order to use them in create booking (admin side)
        public async Task<BookingResponseModel> CreateBookingsFromAdminAsync(int customerId, List<CelebrityAdRequestModel> models)
        {
            if (models == null || !models.Any()) throw new CustomException("No ads to create.");

            using var tx = TransactionScopeHelper.GetInstance();

            var newBooking = new Booking
            {
                CustomerId = customerId,
                Status = BookingStatusEnum.Pending,
                CreatedDate = DateTime.UtcNow,
                TrackingId = RandomStringGenerator.GenerateTrackingId("STRBO"),

            };


            _db.Bookings.Add(newBooking);
            await _db.SaveChangesAsync().ConfigureAwait(false);

            var allAds = new List<CelebrityAdvertisement>();
            var allAdQuestions = new List<CelebrityAdvertismentQuestion>();
            var allAdHistories = new List<CelebrityAdHistory>();

            foreach (var model in models)
            {

                var adSchedule = await _db.CelebritySchedules
                           .Where(cs => cs.Id == model.CelebrityScheduleId && cs.CelebrityId == model.CelebrityId)
                           .FirstOrDefaultAsync()
                           .ConfigureAwait(false)
                   ?? throw new CustomException("Celebrity schedule not found for the specified celebrity.");

                if (adSchedule.Date < DateOnly.FromDateTime(DateTime.UtcNow))
                {
                    throw new CustomException("Cannot book a past schedule date.");
                }

                bool isScheduleUsed = await _db.CelebrityAdvertisements
                                                .AnyAsync(ca => ca.CelebrityScheduleId == model.CelebrityScheduleId)
                                                .ConfigureAwait(false);

                if (isScheduleUsed) throw new CustomException("The schedule date has already been used in another booking.");

                var companyType = await _db.CompanyTypes.FindAsync(model.CompanyTypeId).ConfigureAwait(false)
                                 ?? throw new CustomException("Company Type not found");
                var country = await _db.Countries.FindAsync(model.CountryId).ConfigureAwait(false)
                              ?? throw new CustomException("Country not found");

                // Retrieve the celebrity’s prices based on the delivery type
                var celebrity = await _db.Celebrities
                                         .Where(c => c.Id == model.CelebrityId)
                                         .Select(c => new
                                         {
                                             DeliveryPrice = c.PricePerDelivery,
                                             PostPrice = c.PricePerPost,
                                             EventPrice = c.PricePerEvent
                                         })
                                         .FirstOrDefaultAsync()
                                         .ConfigureAwait(false);

                if (celebrity == null) throw new CustomException("Celebrity not found");


                decimal adPrice = model.DeliveryType switch
                {
                    DeliveryTypeEnum.Delivery => celebrity.DeliveryPrice,
                    DeliveryTypeEnum.Post => celebrity.PostPrice,
                    DeliveryTypeEnum.Event => celebrity.EventPrice,
                    _ => throw new CustomException("Invalid delivery type specified")
                };

                var celebrityAd = new CelebrityAdvertisement
                {
                    TrackingId = RandomStringGenerator.GenerateTrackingId("STRAD"),
                    BookingId = newBooking.Id,
                    CustomerId = newBooking.CustomerId,
                    CelebrityScheduleId = model.CelebrityScheduleId,
                    CelebrityId = model.CelebrityId,
                    CompanyTypeId = model.CompanyTypeId,
                    CountryId = model.CountryId,
                    CompanyName = model.CompanyName,
                    ContactPerson = model.ContactPerson,
                    ManagerPhone = model.ManagerPhone,
                    DeliveryType = model.DeliveryType,
                    AdDate = adSchedule.Date.ToDateTime(adSchedule.From),
                    AdPrice = adPrice,
                    Status = BookingStatusEnum.Pending
                };


                allAds.Add(celebrityAd);
            }
            // Save ads to generate IDs
            _db.CelebrityAdvertisements.AddRange(allAds);
            await _db.SaveChangesAsync().ConfigureAwait(false);

            //Maintain BookingHistory for the booking status

            //var bookingHistory = new BookingHistory
            //{
            //    BookingId = newBooking.Id,
            //    Status = BookingStatusEnum.Pending,
            //    CreatedDate = DateTime.UtcNow,
            //    Comment = "Booking is created"
            //};
            //_db.BookingHistories.Add(bookingHistory);

            // Add questions directly to each ad
            foreach (var model in models)

            {
                var savedAd = allAds.First(a => a.CelebrityScheduleId == model.CelebrityScheduleId && a.CustomerId == newBooking.CustomerId);

                // Add CelebrityAdHistory for each new ad with status Pending
                //var adHistory = new CelebrityAdHistory
                //{
                //    AdId = savedAd.Id,
                //    Status = BookingStatusEnum.Pending,
                //    CreatedDate = DateTime.UtcNow,
                //    Comment = "Advertisement is created"
                //};
                //allAdHistories.Add(adHistory);

                foreach (var question in model.Questions)
                {
                    var questionOccured = await _db.QuestionSettings.FindAsync(question.QuestionId).ConfigureAwait(false)
                        ?? throw new CustomException("Question not found");

                    QuestionAnswerValidator.ValidateAnswerType(questionOccured, question);

                    // Check if the selected option exists
                    if (question.SelectedOptionId.HasValue)
                    {
                        var selectOptionOccured = await _db.AnswerOptions.FindAsync(question.SelectedOptionId.Value).ConfigureAwait(false);
                        if (selectOptionOccured == null) throw new CustomException("Select Option not found");
                    }

                    // Create ad question entries

                    var adQuestion = new CelebrityAdvertismentQuestion
                    {
                        AdId = savedAd.Id,
                        QuestionId = question.QuestionId,
                        TextAnswer = question.TextAnswer,
                        SelectedOptionId = question.SelectedOptionId,
                        DateAnswer = question.DateAnswer,
                        NumberAnswer = question.NumberAnswer,
                    };
                    allAdQuestions.Add(adQuestion);
                }
            }

            // Save questions
            _db.CelebrityAdvertismentQuestions.AddRange(allAdQuestions);
            _db.CelebrityAdHistories.AddRange(allAdHistories);
            await _db.SaveChangesAsync().ConfigureAwait(false);

            tx.Complete();

            // Build response
            return new BookingResponseModel
            {
                BookingId = newBooking.Id,
                CustomerId = newBooking.CustomerId,
                Status = newBooking.Status.ToString(),
                CreatedDate = newBooking.CreatedDate
            };
        }

        public async Task<CelebrityAdResponseModel> UpdateCelebrityAdvertisementFromAdminAsync(int adId, CelebrityAdRequestModel model)
        {
            using var tx = TransactionScopeHelper.GetInstance();

            var ad = await _db.CelebrityAdvertisements
                              .Include(a => a.CelebrityAdvertismentQuestions) // Include existing questions for updates
                              .FirstOrDefaultAsync(a => a.Id == adId).ConfigureAwait(false)
                              ?? throw new CustomException("Advertisement not found");

            if (ad.Status == BookingStatusEnum.Cancelled)
                throw new CustomException("Cannot update a cancelled advertisement.");

            var adSchedule = await _db.CelebritySchedules.FindAsync(model.CelebrityScheduleId).ConfigureAwait(false) ?? throw new CustomException("Celebrity schedule not found");
            if (adSchedule.Date < DateOnly.FromDateTime(DateTime.UtcNow)) throw new CustomException("Cannot update to a past schedule date.");

            bool isScheduleUsed = await _db.CelebrityAdvertisements
                                           .AnyAsync(ca => ca.CelebrityScheduleId == model.CelebrityScheduleId && ca.Id != adId).ConfigureAwait(false);
            if (isScheduleUsed) throw new CustomException("The schedule date has already been used in another advertisement.");

            var companyTypes = await _db.CompanyTypes.FindAsync(model.CompanyTypeId).ConfigureAwait(false) ?? throw new CustomException("Company Type not found");
            var countries = await _db.Countries.FindAsync(model.CountryId).ConfigureAwait(false) ?? throw new CustomException("Country not found");

            var celebrity = await _db.Celebrities
                                     .Where(c => c.Id == model.CelebrityId)
                                     .Select(c => new
                                     {
                                         DeliveryPrice = c.PricePerDelivery,
                                         PostPrice = c.PricePerPost,
                                         EventPrice = c.PricePerEvent
                                     })
                                     .FirstOrDefaultAsync()
                                     .ConfigureAwait(false) ?? throw new CustomException("Celebrity not found");

            decimal adPrice = model.DeliveryType switch
            {
                DeliveryTypeEnum.Delivery => celebrity.DeliveryPrice,
                DeliveryTypeEnum.Post => celebrity.PostPrice,
                DeliveryTypeEnum.Event => celebrity.EventPrice,
                _ => throw new CustomException("Invalid delivery type specified")
            };

            ad.CelebrityId = model.CelebrityId;
            ad.CelebrityScheduleId = model.CelebrityScheduleId;
            ad.CompanyTypeId = model.CompanyTypeId;
            ad.CountryId = model.CountryId;
            ad.CompanyName = model.CompanyName;
            ad.ContactPerson = model.ContactPerson;
            ad.ManagerPhone = model.ManagerPhone;
            ad.DeliveryType = model.DeliveryType;
            ad.AdDate = adSchedule.Date.ToDateTime(adSchedule.From);
            ad.AdPrice = adPrice;

            var existingQuestions = ad.CelebrityAdvertismentQuestions.ToList();
            foreach (var questionModel in model.Questions)
            {
                var questionSetting = await _db.QuestionSettings.FindAsync(questionModel.QuestionId).ConfigureAwait(false)
                                      ?? throw new CustomException("Question not found");

                QuestionAnswerValidator.ValidateAnswerType(questionSetting, questionModel);

                var existingQuestion = existingQuestions.FirstOrDefault(q => q.QuestionId == questionModel.QuestionId) ?? throw new CustomException("Existing question not found");

                existingQuestion.TextAnswer = questionModel.TextAnswer;
                existingQuestion.SelectedOptionId = questionModel.SelectedOptionId;
                existingQuestion.DateAnswer = questionModel.DateAnswer;
                existingQuestion.NumberAnswer = questionModel.NumberAnswer;
            }

            //_db.CelebrityAdHistories.Add(new CelebrityAdHistory
            //{
            //    AdId = ad.Id,
            //    Status = ad.Status,
            //    Comment = "Advertisement updated",
            //    UpdatedDate = DateTime.UtcNow
            //});

            await _db.SaveChangesAsync();
            tx.Complete();

            var response = new CelebrityAdResponseModel
            {
                AdvertisementId = ad.Id,
                CompanyName = ad.CompanyName,
                ContactPerson = ad.ContactPerson,
                ManagerPhone = ad.ManagerPhone,
                DeliveryType = ad.DeliveryType.ToString(),
                Status = ad.Status.ToString(),
            };
            return response;
        }

        public async Task CancelBookingByAdAsync(int adId, string reason)
        {
            var ad = await _db.CelebrityAdvertisements
                .FirstOrDefaultAsync(a => a.Id == adId)
                ?? throw new CustomException("Advertisement not found");

            if (ad.Status == BookingStatusEnum.Cancelled)
                throw new CustomException("Advertisement is already cancelled");

            if (string.IsNullOrWhiteSpace(reason))
                throw new CustomException("Cancellation reason is required.");

            // Cancel the specific ad and add entry in CelebrityAdHistory
            ad.Status = BookingStatusEnum.Cancelled;
            AddAdvertismentHistory(ad, "Advertisement Cancelled. " + reason, BookingStatusEnum.Cancelled);

            // Check if all ads in the booking are now cancelled
            var booking = ad.Booking;
            var allAdsCancelled = booking.CelebrityAdvertisements.All(a => a.Status == BookingStatusEnum.Cancelled);

            // If all ads are cancelled, update booking status and add entry in BookingHistory
            if (allAdsCancelled)
            {
                booking.Status = BookingStatusEnum.Cancelled;
                AddBookingHistory(booking, "Booking Cancelled. " + reason, BookingStatusEnum.Cancelled);

            }

            await _db.SaveChangesAsync().ConfigureAwait(false);
        }



        //HELPER METHODS
        private void AddBookingHistory(Booking booking, string comment, BookingStatusEnum status)
        {
            booking.AddBookingHistory(comment, status);
        }

        private void AddAdvertismentHistory(CelebrityAdvertisement advertisment, string comment, BookingStatusEnum status)
        {
            advertisment.AddAdvertismentHistory(comment, status);
        }

    }
}
