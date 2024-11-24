using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Exceptions;
using CommonBoilerPlateEight.Domain.Models;
using CommonBoilerPlateEight.Web.Extensions;

namespace CommonBoilerPlateEight.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Login(string ReturnUrl = "")
        {
            var loginModel = new UserLoginViewModel()
            {
                ReturnUrl = ReturnUrl
            };

            return View(loginModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var user = await _userManager.Users.Where(a => a.UserName == model.UserName).FirstOrDefaultAsync() ?? throw new CustomException("Incorrect Username or Password");

                    var isSucceeded = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, true, true);

                    if (isSucceeded.Succeeded)
                    {
                        this.NotifySuccess("Logged In Successfully");
                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        {

                            return Redirect(model.ReturnUrl);
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    else if (isSucceeded.IsLockedOut)
                    {
                        return RedirectToAction(nameof(LockOut));
                    }
                    else
                    {
                        this.NotifyInfo($"Incorrect Username or Password . No of attemp remaining {5 - user.AccessFailedCount}");

                    }
                }
            }
            catch (CustomException ex)
            {
                this.NotifyInfo(ex.Message);

            }
            catch (Exception ex)
            {
                this.NotifyError("Something went wrong. Please contact to administrator");

            }
            return RedirectToAction(nameof(Login));
        }



        public IActionResult LockOut()
        {
            return View();
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            this.NotifySuccess("Logged Out Successfully");
            return RedirectToAction(nameof(Login));
        }
    }
}