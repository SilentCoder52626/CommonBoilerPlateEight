using Microsoft.AspNetCore.Mvc;
using CommonBoilerPlateEight.Domain.Exceptions;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Web.Extensions;
using System.Net;

namespace CommonBoilerPlateEight.Web.Controllers
{
    public class QuestionController : Controller
    {
        private readonly IQuestionSettingService _questionSettingService;
        public QuestionController(IQuestionSettingService questionSettingService)
        {
            _questionSettingService = questionSettingService;
        }
        public async Task<IActionResult> Index(QuestionSettingFilterViewModel filter)
        {
            var result = await _questionSettingService.GetAllAsPagedList(filter);
            return View(new QuestionSettingIndexAndFilterViewModel
            {
                Filter = filter,
                Results = result
            });
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] QuestionSettingCreateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return this.ApiErrorResponse(HttpStatusCode.BadRequest, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList(), Notify.Info.ToString());
                }
                await _questionSettingService.Create(model);
                return this.ApiSuccessResponse(HttpStatusCode.OK, "Created Successfully");
            }
            catch (BaseException ex)
            {
                return this.ApiErrorResponse(HttpStatusCode.BadRequest, new List<string> { ex.Message }, Notify.Info.ToString());

            }
            catch (Exception)
            {
                return this.ApiErrorResponse(HttpStatusCode.BadRequest, new List<string> { "Failed to create question." }, Notify.Error.ToString());

            }
        }
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var question = await _questionSettingService.GetById(id);
                var editModel = new QuestionSettingEditViewModel
                {
                    Id = question.Id,
                    AnswerType = question.AnswerType.ToString(),
                    Question = question.Question,
                    DeliveryType = question.DeliveryType.ToString(),
                    AnswerOptions = question.AnswerOptions
                };
                return View(editModel);
            }
            catch (Exception ex)
            {
                this.NotifyError("Something went wrong.Please contact to adminsitrator");
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] QuestionSettingEditViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return this.ApiErrorResponse(HttpStatusCode.BadRequest, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList(), Notify.Info.ToString());
                }
                await _questionSettingService.Edit(model);
                return this.ApiSuccessResponse(HttpStatusCode.OK, "Updated Successfully");
            }
            catch (BaseException ex)
            {
                return this.ApiErrorResponse(HttpStatusCode.BadRequest, new List<string> { ex.Message }, Notify.Info.ToString());

            }
            catch (Exception)
            {
                return this.ApiErrorResponse(HttpStatusCode.BadRequest, new List<string> { "Failed to Update question." }, Notify.Error.ToString());

            }
        }
    }
}
