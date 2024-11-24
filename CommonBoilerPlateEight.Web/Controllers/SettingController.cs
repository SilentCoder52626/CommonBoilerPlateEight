using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Web.Extensions;

namespace CommonBoilerPlateEight.Web.Controllers
{
    [Authorize]
    public class SettingController : Controller
    {
        private readonly IApplicationSettingService _settingService;
        public SettingController(IApplicationSettingService settingService)
        {
            _settingService = settingService;
        }
        public async Task<IActionResult> EmailSetup()
        {
            var emailSetupViewModel = await _settingService.GetEmailSettings();
            return View(emailSetupViewModel);
        }
       
        [HttpPost]
        public async Task<IActionResult> EmailSetup(EmailSetupViewModel model)
        {
            await _settingService.SetEmailSettings(model);
            this.NotifySuccess("Saved Successfully");
            return RedirectToAction(nameof(EmailSetup));
        }
    }
}
