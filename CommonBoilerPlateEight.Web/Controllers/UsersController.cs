using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CommonBoilerPlateEight.Domain.Enums;
using CommonBoilerPlateEight.Domain.Exceptions;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models;
using CommonBoilerPlateEight.Web.Extensions;
using System.Net;

namespace CommonBoilerPlateEight.Web.Controllers
{
   
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        public UsersController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }
      
        public async Task<IActionResult> Index(string searchWord, int page = 1)
        {
            if (!string.IsNullOrEmpty(searchWord))
            {
                ViewBag.CurrentSearchWord = searchWord;
            }
            var result = await _userService.GetAllAsPagedList(new AdminUserFilterViewModel
            {
                PageNo = page,
                Search = searchWord
            });

            return View(result);
        }
      
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await SetRolesDropdown();
            return View();
        }
       

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            try
            {        
                var response = await _userService.Create(model);
                this.NotifySuccess("User created Successfully");
                return RedirectToAction(nameof(Index));
            }
            catch (CustomException ex)
            {
                await SetRolesDropdown();
                this.NotifyInfo(ex.Message);
                return View(model);
            }
            catch (Exception)
            {
                await SetRolesDropdown();
                this.NotifyError("Something went wrong. Please contact to administrator");
                return View(model);
            }
        }
       
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                await SetRolesDropdown();
                var userResponse = await _userService.GetById(id);
                var editViewModel = new UpdateUserViewModel
                {
                    Id = id,
                    FullName = userResponse.FullName,
                    EmailAddress = userResponse.EmailAddress,
                    PhoneNumber = userResponse.PhoneNumber,
                    UserName = userResponse.UserName,
                    RoleId = userResponse.RoleId
                };

                return View(editViewModel);
            }
            catch (Exception)
            {
                this.NotifyError("Something went wrong. Please contact to administrator");
                return RedirectToAction(nameof(Index));
            }

        }

       
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateUserViewModel model)
        {
            try
            {
                var editDto = new UpdateUserViewModel
                {
                    Id = model.Id,
                    FullName = model.FullName,
                    EmailAddress = model.EmailAddress,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.UserName,
                    RoleId = model.RoleId
                };
                await _userService.Edit(editDto);
                this.NotifySuccess("User updated Successfully");
                return RedirectToAction(nameof(Index));
            }
            catch (CustomException ex)
            {
                await SetRolesDropdown();
                this.NotifyInfo(ex.Message);
                return View(model);
            }
            catch (Exception)
            {
                await SetRolesDropdown();
                this.NotifyError("Something went wrong. Please contact to administrator");
                return View(model);
            }
        }
      
        [HttpPatch]
        public async Task<IActionResult> BlockUser(string id)
        {
            try
            {
                await _userService.BlockUser(id);
                return this.ApiSuccessResponse(HttpStatusCode.OK, "Successfully blocked user");
            }
            catch (CustomException ex)
            {
                return this.ApiErrorResponse(HttpStatusCode.BadRequest, new List<string> { ex.Message }, Notify.Info.ToString());
            }
            catch (Exception ex)
            {
                return this.ApiErrorResponse(HttpStatusCode.BadRequest, new List<string> { "Something went wrong. Please contact to administrator" }, Notify.Error.ToString());
            }
        }

        
        [HttpPatch]
        public async Task<IActionResult> UnBlockUser(string id)
        {
            try
            {
                await _userService.UnblockUser(id);
                return this.ApiSuccessResponse(HttpStatusCode.OK, "Successfully unblocked user");
            }
            catch (CustomException ex)
            {
                return this.ApiErrorResponse(HttpStatusCode.BadRequest, new List<string> { ex.Message }, Notify.Info.ToString());
            }
            catch (Exception ex)
            {
                return this.ApiErrorResponse(HttpStatusCode.BadRequest, new List<string> { "Something went wrong. Please contact to administrator" }, Notify.Error.ToString());
            }
        }

        private async Task SetRolesDropdown()
        {
            ViewBag.Roles = await _roleService.GetList();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

      
        [HttpPost]
        public async Task<IActionResult> ChangePassword( ChangePasswordViewModel model)
        {
            try
            {
                await _userService.ChangePassword(model);
                this.NotifySuccess("Password Changed Successfully.");
                return RedirectToAction("Index", "Home");
            }
            catch (CustomException ex)
            {
                this.NotifyInfo(ex.Message);
                return View(model);
            }
            catch (Exception)
            {
                this.NotifyError("Something went wrong. Please contact to administrator");
                return View(model);
            }
        }
       
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            try
            {
                await _userService.ResetPassword(model);
                return this.ApiSuccessResponse(HttpStatusCode.OK, "Password Reset Successfull.");
            }
            catch (CustomException ex)
            {
                return this.ApiErrorResponse(HttpStatusCode.BadRequest, new List<string> { ex.Message }, Notify.Info.ToString());
            }
            catch (Exception)
            {
                return this.ApiErrorResponse(HttpStatusCode.BadRequest, new List<string> { "Something went wrong. Please contact to administrator" }, Notify.Error.ToString());
            }
        }
    }
}