using Microsoft.AspNetCore.Mvc;
using CommonBoilerPlateEight.Domain.Exceptions;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Web.Extensions;

public class WalletController : Controller
{
    private readonly IWalletService _walletService;
    private readonly ICustomerService _customerService;

    public WalletController(IWalletService walletService, ICustomerService customerService)
    {
        _walletService = walletService;
        _customerService = customerService;
    }

    public async Task<IActionResult> Index(WalletFilterViewModel filter)
    {
        await PrepareViewData();
        var wallet = await _walletService.LoadWalletDetailsByFilterAsync(filter);
        return View(new WalletIndexViewModel
        {
            Filter = filter,
            Wallets = wallet

        });
    }

    public async Task<IActionResult> Create()
    {
        await PrepareViewData();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(WalletCreateViewModel model)
    {
        try
        {
            await PrepareViewData();
            if (!ModelState.IsValid)
            {
                this.NotifyModelStateErrors();
                return View(model);
            }
            await _walletService.AddBalanceFromAdminAsync(model);
            return RedirectToAction(nameof(Index));
        }
        catch (CustomException ex)
        {
            this.NotifyInfo(ex.Message);
        }
        catch (Exception ex)
        {
            this.NotifyError("Something went wrong. Please contact to administrator.");

        }

        return View(model);
    }

    private async Task PrepareViewData()
    {
        ViewBag.Customers = await _customerService.GetCustomerForDropdown();
    }

}
