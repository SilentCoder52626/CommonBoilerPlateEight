using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CommonBoilerPlateEight.Api.ApiModel;
using CommonBoilerPlateEight.Api.Extensions;
using CommonBoilerPlateEight.Api.Filters;
using CommonBoilerPlateEight.Domain.Extensions;
using CommonBoilerPlateEight.Domain.Interfaces;
using System.Net;

namespace CommonBoilerPlateEight.Api.Controllers
{
    [AuthorizeCustomer]
    [Authorize]
    [Route("api/wallets")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)

        {
            _walletService = walletService;
        }


        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetCustomerWallet()
        {
            var customerId = AppHttpContext.ValidateAndGetCustomerId();
            var wallet = await _walletService.LoadWalletOfCustomerAsync(customerId);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Wallet details retrieved successfully.", wallet);

        }

        
    }
}
