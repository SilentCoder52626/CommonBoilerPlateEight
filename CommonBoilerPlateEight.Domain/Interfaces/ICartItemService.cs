using CommonBoilerPlateEight.Domain.Models;
using CommonBoilerPlateEight.Domain.Models.Cart;

namespace CommonBoilerPlateEight.Domain.Interfaces
{
    public interface ICartItemService
    {
        Task<decimal> CalculateCartTotalAsync(int customerId);
        Task<CartItemBasicResponseModel> CreateCartItemAsync(int customerId, CelebrityAdRequestModel model);
        Task DeleteCartItemAsync(int cartItemId);
        Task<CartItemDetailedResponseModel> GetCartItemByIdAsync(int cartItemId);
        Task<List<CartItemDetailedResponseModel>> GetCartItemsByCustomerAsync(int customerId);
        Task<CartItemDetailedResponseModel> UpdateCartItemAsync(int customerId, int cartItemId, CelebrityAdRequestModel model);
    }
}
