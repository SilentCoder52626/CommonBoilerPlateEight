using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CommonBoilerPlateEight.Domain.Exceptions;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Services;
using CommonBoilerPlateEight.Web.Extensions;
using CommonBoilerPlateEight.Web.ViewModel;
using System.Net;

namespace CommonBoilerPlateEight.Web.Controllers
{
    public class CelebrityController : Controller
    {
        private readonly ICountryService _countryService;
        private readonly ICelebrityTypeService _celebrityTypeService;
        private readonly ICelebrityService _celebrityService;
        public CelebrityController(ICountryService countryService,
            ICelebrityTypeService celebrityTypeService,
            ICelebrityService celebrityService)
        {
            _celebrityTypeService = celebrityTypeService;
            _countryService = countryService;
            _celebrityService = celebrityService;
        }
        public async Task<IActionResult> Index(CelebrityFilterViewModel filter)
        {
            await PrepareViewBags();
            var result = await _celebrityService.GetAllAsPagedList(filter);
            return View(new CelebrityIndexAndFilterViewModel
            {
                Filter = filter,
                Results = result
            });
        }

        public async Task<IActionResult> Pending(CelebrityFilterViewModel filter)
        {
            await PrepareViewBags();
            filter.Status = StatusTypeEnum.Pending.ToString();
            var result = await _celebrityService.GetAllAsPagedList(filter);
            return View(new CelebrityIndexAndFilterViewModel
            {
                Filter = filter,
                Results = result
            });
        }

        public async Task<IActionResult> Approved(CelebrityFilterViewModel filter)
        {
            await PrepareViewBags();
            filter.Status = StatusTypeEnum.Approved.ToString();
            var result = await _celebrityService.GetAllAsPagedList(filter);
            return View(new CelebrityIndexAndFilterViewModel
            {
                Filter = filter,
                Results = result
            });
        }

        public async Task<IActionResult> Rejected(CelebrityFilterViewModel filter)
        {
            await PrepareViewBags();
            filter.Status = StatusTypeEnum.Rejected.ToString();
            var result = await _celebrityService.GetAllAsPagedList(filter);
            return View(new CelebrityIndexAndFilterViewModel
            {
                Filter = filter,
                Results = result
            });
        }
        [HttpPost]
        public async Task<IActionResult> GetFilteredCelebrity([FromForm] CelebrityIndexFilterViewModel model)
        {


            var response = await _celebrityService.GetFilteredCelebrity(model.start, model.length, model.Search);
            var jsonData = new { draw = model.draw, recordsFiltered = response.TotalCount, recordsTotal = response.TotalCount, data = response.Data };
            return Ok(jsonData);
        }

        public async Task<IActionResult> Create()
        {
            await PrepareViewBags().ConfigureAwait(false);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CelebrityCreateViewModel model)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    this.NotifyModelStateErrors();
                    return View(model);
                }
                var celebrityId = await _celebrityService.CreateFromAdmin(model);
                this.NotifySuccess("Created Successfully");
                return RedirectToAction(nameof(Edit), new { id = celebrityId });
            }
            catch (CustomException ex)
            {
                this.NotifyInfo(ex.Message);

            }
            catch (Exception)
            {
                this.NotifyError("Something went wrong. Please contact to administrator");
            }
            await PrepareViewBags();
            return View(model);


        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                await PrepareViewBags();
                var celebrityResponse = await _celebrityService.GetById(id);
                var editViewModel = new CelebrityEditViewModel
                {
                    Id = celebrityResponse.Id,
                    FullName = celebrityResponse.FullName,
                    MobileNumber = celebrityResponse.MobileNumber,
                    CountryId = celebrityResponse.CountryId,
                    Email = celebrityResponse.Email,
                    Gender = celebrityResponse.Gender,
                    ProfileImage = celebrityResponse.ProfileImage,
                    TimeToCall = celebrityResponse.TimeToCall,
                    PricePerPost = celebrityResponse.PricePerAdPost,
                    PricePerDelivery = celebrityResponse.PricePerDelivery,
                    PricePerEvent = celebrityResponse.PricePerEvent,
                    PriceRange = celebrityResponse.PriceRange,
                    CelebrityTypeId = celebrityResponse.CelebrityTypeId,
                    Status = celebrityResponse.Status,
                    Description = celebrityResponse.Description,
                    FacebookLink = celebrityResponse.SocialLink.FacebookLink,
                    InstagramLink = celebrityResponse.SocialLink.InstagramLink,
                    SnapchatLink = celebrityResponse.SocialLink.SnapchatLink,
                    ThreadsLink = celebrityResponse.SocialLink.ThreadsLink,
                    YoutubeLink = celebrityResponse.SocialLink.YoutubeLink,
                    TwitterLink = celebrityResponse.SocialLink.TwitterLink,
                    CivilIdAttachment = celebrityResponse.CivilIdAttachment,
                    ContractAttachment = celebrityResponse.ContractAttachment
                };
                return View(editViewModel);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditBasicDetail(CelebrityEditBasicDetailViewModel model)
        {
            try
            {
                await _celebrityService.EditBasicDetail(model);
                this.NotifySuccess("Updated Successfully");
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
            return RedirectToAction(nameof(Edit), new { id = model.Id });
        }

        public async Task<IActionResult> ViewDetails(int id)
        {
            try
            {
                var celebrity = await _celebrityService.GetById(id);
                return View(celebrity);
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


        [HttpPost]
        public async Task<IActionResult> EditSocialLink(CelebritySocialLinkUpdateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    this.NotifyModelStateErrors();
                    return RedirectToAction(nameof(Edit), new { id = model.Id });
                }
                await _celebrityService.EditSocialLink(model);
                this.NotifySuccess("Updated Successfully");
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
            return RedirectToAction(nameof(Edit), new { id = model.Id });
        }


        public async Task<IActionResult> UploadImage(IFormFile File, string Type, int CelebrityId)
        {
            try
            {
                await _celebrityService.UploadAttachment(File, Type, CelebrityId);
                return this.ApiSuccessResponse(System.Net.HttpStatusCode.OK, "Saved Successfully");

            }
            catch (CustomException ex)
            {
                return this.ApiErrorResponse(System.Net.HttpStatusCode.BadRequest, new List<string> { ex.Message }, Notify.Info.ToString());
            }
            catch (Exception)
            {

                return this.ApiErrorResponse(System.Net.HttpStatusCode.BadRequest, new List<string> { "Error on removing attachment" }, Notify.Error.ToString());

            }
        }

        [HttpPatch]
        public async Task<IActionResult> DeleteAttachment(int id)
        {
            try
            {
                await _celebrityService.DeleteAttachment(id);
                return this.ApiSuccessResponse(System.Net.HttpStatusCode.OK, "Deleted Successfully");

            }
            catch (CustomException ex)
            {
                return this.ApiErrorResponse(System.Net.HttpStatusCode.BadRequest, new List<string> { ex.Message }, Notify.Info.ToString());
            }
            catch (Exception)
            {

                return this.ApiErrorResponse(System.Net.HttpStatusCode.BadRequest, new List<string> { "Error on removing attachment" }, Notify.Error.ToString());

            }

        }

        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                await _celebrityService.Approve(id);
                return this.ApiSuccessResponse(HttpStatusCode.OK, "Approved Successfully");
            }
            catch (BaseException ex)
            {
                return this.ApiErrorResponse(HttpStatusCode.BadRequest, new List<string> { ex.Message }, Notify.Info.ToString());

            }
            catch (Exception)
            {
                return this.ApiErrorResponse(HttpStatusCode.BadRequest, new List<string> { "Failed to approve celebrity." }, Notify.Error.ToString());

            }

        }


        [HttpPatch]
        public async Task<IActionResult> Reject(int id, string comment)
        {
            await _celebrityService.Reject(id, comment);
            return this.ApiSuccessResponse(HttpStatusCode.OK, "Rejected Successfully");
        }
        private async Task PrepareViewBags()
        {
            ViewBag.CelebrityTypes = await _celebrityTypeService.GetAllAsync().ConfigureAwait(false);
            ViewBag.Countries = await _countryService.GetAllAsync().ConfigureAwait(false);
        }


        [HttpPatch]
        public async Task<IActionResult> Activate(int id)
        {
            try
            {
                await _celebrityService.Activate(id);
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
                await _celebrityService.Deactivate(id);
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
    }
}
