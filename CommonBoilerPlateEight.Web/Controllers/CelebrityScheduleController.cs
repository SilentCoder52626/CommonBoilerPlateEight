using Microsoft.AspNetCore.Mvc;
using CommonBoilerPlateEight.Domain.Exceptions;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Web.Extensions;

namespace CommonBoilerPlateEight.Web.Controllers
{
    public class CelebrityScheduleController : Controller
    {
        private readonly ICelebrityScheduleService _celebrityScheduleService;
        private readonly ICelebrityService _celebrityService;
        public CelebrityScheduleController(ICelebrityScheduleService celebrityScheduleService, ICelebrityService celebrityService)
        {
            _celebrityScheduleService = celebrityScheduleService;
            _celebrityService = celebrityService;

        }
        public async Task<IActionResult> Index(CelebrityScheduleFilterViewModel filter)
        {
            await PrepareViewData();
            var result = await _celebrityScheduleService.GetAllAsPagedList(filter);
            return View(new CelebrityScheduleIndexAndFilterViewModel
            {
                Filter = filter,
                Results = result
            });
        }

        public async Task<IActionResult> Create()
        {
            await PrepareViewData();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CelebrityScheduleCreateViewModel model)
        {
            try
            {
                await PrepareViewData();
                if (!ModelState.IsValid)
                {
                    this.NotifyModelStateErrors();
                    return View(model);
                }
                await _celebrityScheduleService.Create(model, true);
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
            ViewBag.Celebrities = await _celebrityService.GetCelebrityForDropdown();
        }

        public async Task<IActionResult> Edit(int id)
        {

            var schedule = await _celebrityScheduleService.GetById(id);
            var updateViewModel = new CelebrityScheduleUpdateViewModel
            {
                Id = id,
                FromTime = schedule.FromTime,
                ToTime = schedule.ToTime,
                Date = schedule.Date,
            };
            return View(updateViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CelebrityScheduleUpdateViewModel model)
        {
            try
            {
                await _celebrityScheduleService.Edit(model);
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
    }
}
