using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using CommonBoilerPlateEight.Domain.Enums;
using CommonBoilerPlateEight.Domain.Exceptions;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Services;
using CommonBoilerPlateEight.Web.Extensions;
using System.Net;

namespace CommonBoilerPlateEight.Web.Controllers
{
    public class CelebrityTypesController : Controller
    {
        private readonly ICelebrityTypeService _celebrityTypeService;
        public CelebrityTypesController(ICelebrityTypeService celebrityTypeService)
        {
            _celebrityTypeService = celebrityTypeService;
        }
        public async Task<IActionResult> Index(string searchWord, int page = 1)
        {
            if (!string.IsNullOrEmpty(searchWord))
            {
                ViewBag.CurrentSearchWord = searchWord;
            }
            var result = await _celebrityTypeService.GetAllAsPagedList(searchWord, pageNumber: page);
            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CelebrityTypeCreateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    this.NotifyModelStateErrors();
                    return View(model);
                }
                await _celebrityTypeService.Create(model);
                this.NotifySuccess("Created Successfully");
                return RedirectToAction(nameof(Index));
            }
            catch (CustomException ex)
            {
                this.NotifyInfo(ex.Message);

            }
            catch (Exception)
            {
                this.NotifyError("Something went wrong. Please contact to administrator");
            }
            return View(model);

        }

        public async Task<IActionResult> Update(int id)
        {
            var celebrityType = await _celebrityTypeService.GetById(id);
            var editModel = new CelebrityTypeEditViewModel { 
            Id = celebrityType.Id,
            Name = celebrityType.Name
            };

            return View(editModel);
        }
        [HttpPost]
        public async Task<IActionResult> Update(CelebrityTypeEditViewModel request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    this.NotifyModelStateErrors();
                    return View(request);
                }
                await _celebrityTypeService.Edit(request);
                this.NotifySuccess("Updated Successfully");
                return RedirectToAction(nameof(Index));
            }
            catch (CustomException ex)
            {
                this.NotifyInfo(ex.Message);
            }
            catch (Exception)
            {
                this.NotifyError("Something went wrong. Please contact to administrator");
            }
            return View(request);
            
        }

        [HttpPatch]
        public async Task<IActionResult> Activate(int id)
        {
            try
            {
                await _celebrityTypeService.Activate(id);
                return this.ApiSuccessResponse(HttpStatusCode.OK, "Successfully Activated");
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
        public async Task<IActionResult> Deactivate(int id)
        {
            try
            {
                await _celebrityTypeService.Deactivate(id);
                return this.ApiSuccessResponse(HttpStatusCode.OK, "Successfully Deactivated");
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
    }
}
