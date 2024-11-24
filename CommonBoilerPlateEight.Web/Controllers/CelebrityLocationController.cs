using Microsoft.AspNetCore.Mvc;
using CommonBoilerPlateEight.Domain.Exceptions;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Services;
using CommonBoilerPlateEight.Web.Extensions;

namespace CommonBoilerPlateEight.Web.Controllers
{
    public class CelebrityLocationController : Controller
    {

        private readonly ICelebrityLocationService _celebrityLocationService;
        private readonly ICelebrityService _celebrityService;
        public CelebrityLocationController(ICelebrityLocationService celebrityLocationService,
            ICelebrityService celebrityService)
        {
            _celebrityLocationService = celebrityLocationService;
            _celebrityService = celebrityService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            await PrepareViewData();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CelebrityLocationCreateViewModel model)
        {
            try
            {
                await PrepareViewData();
                if (!ModelState.IsValid)
                {
                    this.NotifyModelStateErrors();
                    return View(model);
                }
                await _celebrityLocationService.Create(model);
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

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var location = await _celebrityLocationService.GetById(id);
                var editModel = new CelebrityLocationEditViewModel
                {
                    Id = location.Id,
                    FullAddress = location.FullAddress,
                    Longitude = location.Longitude,
                    Latitude = location.Latitude,
                    Street = location.Street,
                    Area = location.Area,
                    Block = location.Block,
                    Governorate = location.Governorate,
                    GooglePlusCode = location.GooglePlusCode,
                    Note = location.Note
                };
                return View(editModel);
            }
            catch (CustomException ex)
            {
                this.NotifyInfo(ex.Message);
            }
            catch (Exception ex)
            {
                this.NotifyError("Something went wrong. Please contact to administrator.");

            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CelebrityLocationEditViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    this.NotifyModelStateErrors();
                    return View(model);
                }
                await _celebrityLocationService.Update(model);
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


        public async Task<IActionResult> GetLocationByCelebrityId(int celebrityId)
        {
            var response = await _celebrityLocationService.GetAllLocationsOfcelebrity(celebrityId);
            var jsonData = new { recordsFiltered = response.Count(), recordsTotal = response.Count, data = response };
            return Ok(jsonData);
        }
        private async Task PrepareViewData()
        {
            ViewBag.Celebrities = await _celebrityService.GetCelebrityForDropdown();
        }

    }
}
