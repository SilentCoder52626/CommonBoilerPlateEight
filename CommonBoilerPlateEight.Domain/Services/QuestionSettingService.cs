using Microsoft.EntityFrameworkCore;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Enums;
using CommonBoilerPlateEight.Domain.Exceptions;
using CommonBoilerPlateEight.Domain.Extensions;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Domain.Models;
using X.PagedList;

namespace CommonBoilerPlateEight.Domain.Services
{
    public class QuestionSettingService : IQuestionSettingService
    {
        private readonly IDbContext _db;
        public QuestionSettingService(IDbContext db)
        {
            _db = db;
        }

        public async Task Activate(int id)
        {
            var questionSetting = await _db.QuestionSettings.Include(a => a.AnswerOptions).FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false) ?? throw new CustomException("Question not found");
            questionSetting.Activate();
            _db.QuestionSettings.Update(questionSetting);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Create(QuestionSettingCreateViewModel model)
        {
            var question = new QuestionSetting
            {
                Question = model.Question,
                AnswerType = model.AnswerType.ToEnum<AnswerTypeEnum>(),
                DeliveryType = model.DeliveryType.ToEnum<DeliveryTypeEnum>(),
                IsActive = true
            };
            if (model.AnswerType == AnswerTypeEnum.Dropdown.ToString())
            {
                if (!model.AnswerOptions.Any()) throw new CustomException("Dropdown values are required for dropdown type.");
                foreach (var option in model.AnswerOptions)
                {
                    question.AddQuestionChoice(option.AnswerOption);
                }
            }
            await _db.QuestionSettings.AddAsync(question);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Deactivate(int id)
        {
            var questionSetting = await _db.QuestionSettings.Include(a => a.AnswerOptions).FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false) ?? throw new CustomException("Question not found");
            questionSetting.Deactivate();
            _db.QuestionSettings.Update(questionSetting);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Edit(QuestionSettingEditViewModel model)
        {
            var questionSetting = await _db.QuestionSettings.Include(a => a.AnswerOptions).FirstOrDefaultAsync(a => a.Id == model.Id).ConfigureAwait(false) ?? throw new CustomException("Question not found");
            questionSetting.Question = model.Question;
            questionSetting.DeliveryType = model.DeliveryType.ToEnum<DeliveryTypeEnum>();
            questionSetting.AnswerType = model.AnswerType.ToEnum<AnswerTypeEnum>();
            var existingOptions = questionSetting.AnswerOptions.Where(a => !a.DeletedDate.HasValue).ToList();
            if (model.AnswerType == AnswerTypeEnum.Dropdown.ToString())
            {
                if (!model.AnswerOptions.Any()) throw new CustomException("Dropdown values are required for dropdown type.");
                var allOptionIds = model.AnswerOptions.Select(a => a.Id).ToList();
                var optionsToDelete = existingOptions
                    .Where(a => !allOptionIds.Contains(a.Id))
                    .ToList();

                foreach (var option in optionsToDelete)
                {
                    option.MarkAsDeleted();
                }

                foreach (var modelOption in model.AnswerOptions)
                {
                    var existingOption = existingOptions
                        .FirstOrDefault(e => e.Id == modelOption.Id);

                    if (existingOption != null)
                    {
                        existingOption.OptionText = modelOption.AnswerOption;
                    }
                    else
                    {
                        questionSetting.AddQuestionChoice(modelOption.AnswerOption);
                    }
                }


            }

            else
            {
                foreach (var option in existingOptions)
                {
                    option.MarkAsDeleted();
                }
            }

            _db.QuestionSettings.Update(questionSetting);
            await _db.SaveChangesAsync().ConfigureAwait(false);

        }

        public async Task<IPagedList<QuestionSettingResponseViewModel>> GetAllAsPagedList(QuestionSettingFilterViewModel model)
        {
            var questionQueryable = _db.QuestionSettings.Where(a => !a.DeletedDate.HasValue).AsQueryable();
            if (!string.IsNullOrWhiteSpace(model.AnswerType))
            {
                var answerType = model.AnswerType.ToEnum<AnswerTypeEnum>();
                questionQueryable = questionQueryable.Where(a => a.AnswerType == answerType);
            }
            if (!string.IsNullOrWhiteSpace(model.DeliveryType))
            {
                var deliveryType = model.DeliveryType.ToEnum<DeliveryTypeEnum>();
                questionQueryable = questionQueryable.Where(a => a.DeliveryType == deliveryType);
            }
            var questions = await questionQueryable.OrderBy(a => a.Id).Select(a => new QuestionSettingResponseViewModel
            {
                Id = a.Id,
                AnswerType = a.AnswerType.ToString(),
                DeliveryType = a.DeliveryType.ToString(),
                Question = a.Question,
                IsActive = a.IsActive
            }).ToPagedListAsync(model.PageNumber, model.pageSize).ConfigureAwait(false);
            return questions;
        }

        public async Task<QuestionSettingResponseViewModel> GetById(int id)
        {
            var questionSetting = await _db.QuestionSettings.Include(a => a.AnswerOptions).FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false) ?? throw new CustomException("Question not found");

            return new QuestionSettingResponseViewModel
            {
                Id = questionSetting.Id,
                AnswerType = questionSetting.AnswerType.ToString(),
                Question = questionSetting.Question,
                DeliveryType = questionSetting.DeliveryType.ToString(),
                IsActive = questionSetting.IsActive,
                AnswerOptions = questionSetting.AnswerOptions.Where(a => !a.DeletedDate.HasValue).Select(a => new AnswerOptionViewModel
                {
                    Id = a.Id,
                    AnswerOption = a.OptionText
                }).ToList()
            };
        }
    }
}
