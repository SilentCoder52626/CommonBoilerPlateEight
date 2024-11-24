using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CommonBoilerPlateEight.Api.ApiModel;
using CommonBoilerPlateEight.Api.Extensions;
using CommonBoilerPlateEight.Api.Filters;
using CommonBoilerPlateEight.Domain.Extensions;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models;
using System.Net;

namespace CommonBoilerPlateEight.Api.Controllers
{
    [AuthorizeCustomer]
    [Authorize]
    [Route("api/cart-items")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;

        public CartController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;

        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetCartItemById(int id)
        {
            var cartItemResponse = await _cartItemService.GetCartItemByIdAsync(id);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Cart item retrieved successfully.", cartItemResponse);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetCartItemsByCustomerAsync()
        {
            var customerId = AppHttpContext.ValidateAndGetCustomerId();
            var cartItemResponse = await _cartItemService.GetCartItemsByCustomerAsync(customerId);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Cart item retrieved successfully.", cartItemResponse);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateCartItem([FromBody] CelebrityAdRequestModel model)
        {
            var customerId = AppHttpContext.ValidateAndGetCustomerId();
            var cartItemResponse = await _cartItemService.CreateCartItemAsync(customerId, model);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Cart item created successfully.", cartItemResponse);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateCartItem(int id, [FromBody] CelebrityAdRequestModel model)
        {
            var customerId = AppHttpContext.ValidateAndGetCustomerId();
            var cartItemResponse = await _cartItemService.UpdateCartItemAsync(customerId, id, model);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Cart item updated successfully.", cartItemResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartItem(int id)
        {

            await _cartItemService.DeleteCartItemAsync(id);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Cart item deleted successfully.");

        }


    }

}

