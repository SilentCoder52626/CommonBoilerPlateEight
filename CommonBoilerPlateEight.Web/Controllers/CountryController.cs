using Microsoft.AspNetCore.Mvc;
using CommonBoilerPlateEight.Domain.Exceptions;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Services;
using CommonBoilerPlateEight.Web.Extensions;

namespace CommonBoilerPlateEight.Web.Controllers
{
    public class CountryController : Controller
    {
        private readonly ICountryService _countryService;
        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }
        public async Task<IActionResult> Index(string searchWord, int page = 1)
        {
            if (!string.IsNullOrEmpty(searchWord))
            {
                ViewBag.CurrentSearchWord = searchWord;
            }
            var result = await _countryService.GetAllAsPagedList(searchWord, pageNumber: page);
            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CountryCreateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    this.NotifyModelStateErrors();
                    return View(model);
                }
                await _countryService.Create(model);
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
            var celebrityType = await _countryService.GetById(id);
            var editModel = new CountryEditViewModel
            {
                Id = celebrityType.Id,
                Name = celebrityType.Name
            };

            return View(editModel);
        }
        [HttpPost]
        public async Task<IActionResult> Update(CountryEditViewModel request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    this.NotifyModelStateErrors();
                    return View(request);
                }
                await _countryService.Edit(request);
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
    }
}
