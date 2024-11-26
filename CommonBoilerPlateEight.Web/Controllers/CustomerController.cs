using Microsoft.AspNetCore.Mvc;
using CommonBoilerPlateEight.Domain.Exceptions;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Services;
using CommonBoilerPlateEight.Web.Extensions;
using System.Net;

namespace CommonBoilerPlateEight.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICountryService _countryService;
        private readonly ICelebrityTypeService _celebrityTypeService;
        private readonly ICustomerService _customerService;

        public CustomerController(ICountryService countryService,
            ICelebrityTypeService celebrityTypeService,
            ICustomerService customerService
            )
        {
            _celebrityTypeService = celebrityTypeService;
            _countryService = countryService;
            _customerService = customerService;
        }
        public async Task<IActionResult> Index(CustomerFilterViewModel filter)
        {
            await PrepareViewBags();
            var result = await _customerService.GetAllAsPagedList(filter);
            return View(new CustomerIndexAndFilterViewModel
            {
                Filter = filter,
                Results = result
            });
           
        }

        public async Task<IActionResult> Create()
        {
            await PrepareViewBags();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CustomerCreateViewModel model)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    this.NotifyModelStateErrors();
                    return View(model);
                }
                var customerId = await _customerService.Create(model);
                this.NotifySuccess("Created Successfully");
                return RedirectToAction(nameof(Edit), new { id = customerId });
            }
            catch (CustomException ex)
            {
                this.NotifyInfo(ex.Message);

            }
            catch (Exception)
            {
                this.NotifyError("Something went wrong. Please contact to administrator");
            }
            await PrepareViewBags().ConfigureAwait(false);
            return View(model);


        }
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                await PrepareViewBags();
                var celebrityResponse = await _customerService.GetById(id);
                var editViewModel = new CustomerEditViewModel
                {
                    Id = celebrityResponse.Id,
                    FullName = celebrityResponse.FullName,
                    MobileNumber = celebrityResponse.MobileNumber,
                    CountryId = celebrityResponse.CountryId,
                    Email = celebrityResponse.Email,
                    Gender = celebrityResponse.Gender,
                    ProfileImage = celebrityResponse.ProfileImage,
                    Description = celebrityResponse.Description
                };
                return View(editViewModel);
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CustomerEditViewModel model)
        {
            try
            {
                await _customerService.Edit(model);
                this.NotifySuccess("Updated Successfully");
                return RedirectToAction(nameof(Index));
            }
            catch (CustomException ex)
            {
                this.NotifyInfo(ex.Message);
                await PrepareViewBags().ConfigureAwait(false);
                return View(model);

            }
            catch (Exception)
            {
                this.NotifyError("Something went wrong. Please contact to administrator");
                return RedirectToAction(nameof(Index));
            }
          
        }

        public async Task<IActionResult> ViewDetails(int id)
        {
            try
            {
                var customer = await _customerService.GetById(id);
                return View(customer);
            }
            catch (CustomException ex)
            {
                this.NotifyInfo(ex.Message);

            }
            catch (Exception)
            {
                this.NotifyError("Something went wrong. Please contact to administrator");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPatch]
        public async Task<IActionResult> Activate(int id)
        {
            try
            {
                await _customerService.Activate(id);
                return this.ApiSuccessResponse(HttpStatusCode.OK, "Successfully activated.");
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
                await _customerService.Deactivate(id);
                return this.ApiSuccessResponse(HttpStatusCode.OK, "Successfully deactivated");
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

        private async Task PrepareViewBags()
        {
            ViewBag.Countries = await _countryService.GetAllAsync().ConfigureAwait(false);
        }
    }
}
