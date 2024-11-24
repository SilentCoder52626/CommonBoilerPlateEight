using Microsoft.EntityFrameworkCore;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Enums;
using CommonBoilerPlateEight.Domain.Exceptions;
using CommonBoilerPlateEight.Domain.Helper;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models;
using CommonBoilerPlateEight.Domain.Models.Cart;

namespace CommonBoilerPlateEight.Domain.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly IDbContext _db;
        public CartItemService(IDbContext db)
        {
            _db = db;
        }
        public async Task<CartItemDetailedResponseModel> GetCartItemByIdAsync(int cartItemId)
        {
            var cartItem = await _db.CartItems
            .AsNoTracking()
            .Where(ci => ci.Id == cartItemId)
            .Select(cartItem => new CartItemDetailedResponseModel
            {
                CartItemId = cartItem.Id,
                CompanyName = cartItem.CompanyName,
                CelebrityName = cartItem.Celebrity.FullName,
                ContactPerson = cartItem.ContactPerson,
                ManagerPhone = cartItem.ManagerPhone,
                DeliveryType = cartItem.DeliveryType.ToString(),
                AdDate = cartItem.AdDate,
                AdPrice = cartItem.AdPrice,
            })
            .FirstOrDefaultAsync()
            .ConfigureAwait(false) ?? throw new CustomException("Cart item not found");

            return cartItem;
        }

        public async Task<List<CartItemDetailedResponseModel>> GetCartItemsByCustomerAsync(int customerId)
        {
            var cartItems = await _db.CartItems
            .AsNoTracking()
            .Where(ci => ci.CustomerId == customerId)
            .Select(cartItem => new CartItemDetailedResponseModel
            {
                CartItemId = cartItem.Id,
                CompanyName = cartItem.CompanyName,
                CelebrityName = cartItem.Celebrity.FullName,
                ContactPerson = cartItem.ContactPerson,
                ManagerPhone = cartItem.ManagerPhone,
                DeliveryType = cartItem.DeliveryType.ToString(),
                AdDate = cartItem.AdDate,
                AdPrice = cartItem.AdPrice,
            })
            .ToListAsync()
            .ConfigureAwait(false);

            return cartItems;
        }

        public async Task<decimal> CalculateCartTotalAsync(int customerId)
        {
            return await _db.CartItems
                .Where(ci => ci.CustomerId == customerId)
                .SumAsync(ci => ci.AdPrice);
        }

        public async Task<CartItemBasicResponseModel> CreateCartItemAsync(int customerId, CelebrityAdRequestModel model)
        {
            if (model == null) throw new CustomException("No cart item data provided.");

            using var tx = TransactionScopeHelper.GetInstance();

            var cartItem = await CreateCartItem(customerId, model).ConfigureAwait(false);

            var cartItemQuestions = await CreateCartItemQuestions(cartItem.Id, model.Questions, cartItem.DeliveryType).ConfigureAwait(false);

            tx.Complete();

            return new CartItemBasicResponseModel
            {
                CartItemId = cartItem.Id,
                CustomerId = cartItem.CustomerId,
                CelebrityId = cartItem.CelebrityId,
                CreatedDate = cartItem.CreatedDate
            };
        }

        public async Task<CartItemDetailedResponseModel> UpdateCartItemAsync(int customerId, int cartItemId, CelebrityAdRequestModel model)
        {
            if (model == null) throw new CustomException("No cart item data provided.");

            using var tx = TransactionScopeHelper.GetInstance();

            var cartItem = await _db.CartItems
                .Include(c => c.Celebrity)
                .Include(ci => ci.CartItemQuestions)
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.CustomerId == customerId)
                .ConfigureAwait(false)
                ?? throw new CustomException("Cart item not found or access denied.");


            var companyType = await _db.CompanyTypes.FindAsync(model.CompanyTypeId).ConfigureAwait(false) ?? throw new CustomException("Company Type not found");

            var country = await _db.Countries.FindAsync(model.CountryId).ConfigureAwait(false) ?? throw new CustomException("Country not found");

            var newSchedule = await _db.CelebritySchedules
                .FirstOrDefaultAsync(cs => cs.Id == model.CelebrityScheduleId && cs.CelebrityId == model.CelebrityId)
                .ConfigureAwait(false) ?? throw new CustomException("Celebrity schedule not found for the specified celebrity.");

            if (newSchedule.Date < DateOnly.FromDateTime(DateTime.UtcNow))
                throw new CustomException("Cannot book a past schedule date.");

            bool isScheduleUsed = await _db.CelebrityAdvertisements
                .AnyAsync(ca => ca.CelebrityScheduleId == model.CelebrityScheduleId)
                .ConfigureAwait(false);

            if (isScheduleUsed)
                throw new CustomException("The schedule date has already been used in another booking.");


            if (cartItem.CelebrityScheduleId != model.CelebrityScheduleId)
            {
                cartItem.CelebrityScheduleId = model.CelebrityScheduleId;
                cartItem.AdDate = newSchedule.Date.ToDateTime(newSchedule.From);
            }
            if (cartItem.CompanyTypeId != model.CompanyTypeId) cartItem.CompanyTypeId = model.CompanyTypeId;
            if (cartItem.CountryId != model.CountryId) cartItem.CountryId = model.CountryId;


            // Check if the DeliveryType has changed
            bool deliveryTypeChanged = cartItem.DeliveryType != model.DeliveryType;
            if (deliveryTypeChanged)
            {
                // Remove existing questions for the old delivery type
                _db.CartItemQuestions.RemoveRange(cartItem.CartItemQuestions);
                await _db.SaveChangesAsync().ConfigureAwait(false);

                // Assign the new DeliveryType
                cartItem.DeliveryType = model.DeliveryType;

                // Add and validate new questions according to the new DeliveryType
                foreach (var question in model.Questions)
                {
                    var questionSetting = await _db.QuestionSettings
                        .FindAsync(question.QuestionId)
                        .ConfigureAwait(false) ?? throw new CustomException("Question setting not found");

                    QuestionValidatorHelper.ValidateQuestionByDeliveryType(questionSetting, cartItem.DeliveryType);
                    QuestionAnswerValidator.ValidateAnswerType(questionSetting, question);

                    // Add the question if all validations pass
                    cartItem.CartItemQuestions.Add(new CartItemQuestion
                    {
                        CartItemId = cartItem.Id,
                        QuestionId = question.QuestionId,
                        TextAnswer = question.TextAnswer,
                        SelectedOptionId = question.SelectedOptionId,
                        DateAnswer = question.DateAnswer,
                        NumberAnswer = question.NumberAnswer
                    });
                }
            }

            else
            {
                // Update existing cart item questions without clearing them
                foreach (var existingQuestion in cartItem.CartItemQuestions)
                {
                    var updatedQuestion = model.Questions.FirstOrDefault(q => q.QuestionId == existingQuestion.QuestionId);
                    if (updatedQuestion != null)
                    {
                        // Validate and update only if there is a change in the answer
                        if (existingQuestion.TextAnswer != updatedQuestion.TextAnswer ||
                            existingQuestion.SelectedOptionId != updatedQuestion.SelectedOptionId ||
                            existingQuestion.DateAnswer != updatedQuestion.DateAnswer ||
                            existingQuestion.NumberAnswer != updatedQuestion.NumberAnswer)
                        {
                            // Validate the answer type before updating
                            var questionSetting = await _db.QuestionSettings.FindAsync(existingQuestion.QuestionId).ConfigureAwait(false)
                                ?? throw new CustomException("Question not found");
                            QuestionValidatorHelper.ValidateQuestionByDeliveryType(questionSetting, cartItem.DeliveryType);
                            QuestionAnswerValidator.ValidateAnswerType(questionSetting, updatedQuestion);

                            existingQuestion.TextAnswer = updatedQuestion.TextAnswer ?? existingQuestion.TextAnswer;
                            existingQuestion.SelectedOptionId = updatedQuestion.SelectedOptionId ?? existingQuestion.SelectedOptionId;
                            existingQuestion.DateAnswer = updatedQuestion.DateAnswer ?? existingQuestion.DateAnswer;
                            existingQuestion.NumberAnswer = updatedQuestion.NumberAnswer ?? existingQuestion.NumberAnswer;
                        }
                    }
                }
            }

            cartItem.CompanyName = model.CompanyName ?? cartItem.CompanyName;
            cartItem.ContactPerson = model.ContactPerson ?? cartItem.ContactPerson;
            cartItem.ManagerPhone = model.ManagerPhone ?? cartItem.ManagerPhone;
            cartItem.AdPrice = DetermineAdPriceHelper.DetermineAdPrice(model.DeliveryType, cartItem.Celebrity);


            await _db.SaveChangesAsync().ConfigureAwait(false);
            tx.Complete();


            return new CartItemDetailedResponseModel
            {
                CartItemId = cartItem.Id,
                CelebrityName = cartItem.Celebrity.FullName,
                CompanyName = cartItem.CompanyName,
                ContactPerson = cartItem.ContactPerson,
                ManagerPhone = cartItem.ManagerPhone,
                AdDate = cartItem.AdDate,
                AdPrice = cartItem.AdPrice,
                DeliveryType = cartItem.DeliveryType.ToString(),
                Questions = cartItem.CartItemQuestions.Select(q => new CartItemUpdateResponseModel
                {
                    QuestionId = q.QuestionId,
                    TextAnswer = q.TextAnswer,
                    SelectedOptionId = q.SelectedOptionId,
                    DateAnswer = q.DateAnswer,
                    NumberAnswer = q.NumberAnswer
                }).ToList()
            };
        }


        public async Task DeleteCartItemAsync(int cartItemId)
        {
            var cartItem = await _db.CartItems
                .Include(ci => ci.CartItemQuestions)
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId)
                .ConfigureAwait(false) ?? throw new CustomException("Cart item not found.");

            _db.CartItemQuestions.RemoveRange(cartItem.CartItemQuestions);

            _db.CartItems.Remove(cartItem);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }



        //HELPER METHODS
        private async Task<CartItem> CreateCartItem(int customerId, CelebrityAdRequestModel model)
        {
            var companyType = await _db.CompanyTypes.FindAsync(model.CompanyTypeId).ConfigureAwait(false) ?? throw new CustomException("Company Type not found");

            var country = await _db.Countries.FindAsync(model.CountryId).ConfigureAwait(false) ?? throw new CustomException("Country not found");

            var adSchedule = await _db.CelebritySchedules
              .FirstOrDefaultAsync(cs => cs.Id == model.CelebrityScheduleId && cs.CelebrityId == model.CelebrityId)
              .ConfigureAwait(false) ?? throw new CustomException("Celebrity schedule not found for the specified celebrity.");

            if (adSchedule.Date < DateOnly.FromDateTime(DateTime.UtcNow))
                throw new CustomException("Cannot book a past schedule date.");

            bool isScheduleUsed = await _db.CelebrityAdvertisements
                                        .AnyAsync(ca => ca.CelebrityScheduleId == model.CelebrityScheduleId)
                                        .ConfigureAwait(false);

            if (isScheduleUsed) throw new CustomException("The schedule date has already been used in another booking.");

            var celebrity = await _db.Celebrities
               .Where(c => c.Id == model.CelebrityId)
               .FirstOrDefaultAsync()
               .ConfigureAwait(false) ?? throw new CustomException("Celebrity not found");

            decimal adPrice = DetermineAdPriceHelper.DetermineAdPrice(model.DeliveryType, celebrity);

            var cartItem = new CartItem
            {
                CustomerId = customerId,
                CelebrityId = model.CelebrityId,
                CelebrityScheduleId = model.CelebrityScheduleId,
                CompanyTypeId = model.CompanyTypeId,
                CountryId = model.CountryId,
                CompanyName = model.CompanyName,
                ContactPerson = model.ContactPerson,
                ManagerPhone = model.ManagerPhone,
                AdDate = adSchedule.Date.ToDateTime(adSchedule.From),
                AdPrice = adPrice,
                DeliveryType = model.DeliveryType
            };

            _db.CartItems.Add(cartItem);
            await _db.SaveChangesAsync().ConfigureAwait(false);
            return cartItem;

        }

        private async Task<List<CartItemQuestion>> CreateCartItemQuestions(int cartItemId, List<CelebrityAdQuestionRequestModel> questions, DeliveryTypeEnum deliveryType)
        {
            var cartItemQuestions = new List<CartItemQuestion>();

            foreach (var question in questions)
            {
                var questionSetting = await _db.QuestionSettings.FindAsync(question.QuestionId).ConfigureAwait(false) ?? throw new CustomException("Question not found");

                QuestionValidatorHelper.ValidateQuestionByDeliveryType(questionSetting, deliveryType);

                QuestionAnswerValidator.ValidateAnswerType(questionSetting, question);

                if (question.SelectedOptionId.HasValue)
                {
                    var selectedOption = await _db.AnswerOptions.FindAsync(question.SelectedOptionId.Value).ConfigureAwait(false);
                    if (selectedOption == null) throw new CustomException("Selected option not found");
                }

                var cartItemQuestion = new CartItemQuestion
                {
                    CartItemId = cartItemId,
                    QuestionId = question.QuestionId,
                    TextAnswer = question.TextAnswer,
                    SelectedOptionId = question.SelectedOptionId,
                    DateAnswer = question.DateAnswer,
                    NumberAnswer = question.NumberAnswer
                };

                cartItemQuestions.Add(cartItemQuestion);
            }

            _db.CartItemQuestions.AddRange(cartItemQuestions);
            await _db.SaveChangesAsync().ConfigureAwait(false);

            return cartItemQuestions;
        }



    }
}
