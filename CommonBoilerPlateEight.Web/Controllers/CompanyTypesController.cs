using Microsoft.AspNetCore.Mvc;
using CommonBoilerPlateEight.Domain.Exceptions;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Web.Extensions;
using System.Net;

namespace CommonBoilerPlateEight.Web.Controllers
{
    public class CompanyTypesController : Controller
    {
        private readonly ICompanyTypeService _CompanyTypeService;
        public CompanyTypesController(ICompanyTypeService CompanyTypeService)
        {
            _CompanyTypeService = CompanyTypeService;
        }
        public async Task<IActionResult> Index(string searchWord, int page = 1)
        {
            if (!string.IsNullOrEmpty(searchWord))
            {
                ViewBag.CurrentSearchWord = searchWord;
            }
            var result = await _CompanyTypeService.GetAllAsPagedList(searchWord, pageNumber: page);
            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CompanyTypeCreateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    this.NotifyModelStateErrors();
                    return View(model);
                }
                await _CompanyTypeService.Create(model);
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
            var CompanyType = await _CompanyTypeService.GetById(id);
            var editModel = new CompanyTypeEditViewModel
            {
                Id = CompanyType.Id,
                Name = CompanyType.Name
            };

            return View(editModel);
        }
        [HttpPost]
        public async Task<IActionResult> Update(CompanyTypeEditViewModel request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    this.NotifyModelStateErrors();
                    return View(request);
                }
                await _CompanyTypeService.Edit(request);
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
                await _CompanyTypeService.Activate(id);
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
                await _CompanyTypeService.Deactivate(id);
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
