using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Enums;
using CommonBoilerPlateEight.Domain.Exceptions;
using CommonBoilerPlateEight.Domain.Models;

namespace CommonBoilerPlateEight.Domain.Helper
{
    public static class QuestionAnswerValidator
    {
        public static void ValidateAnswerType(QuestionSetting question, CelebrityAdQuestionRequestModel questionModel)
        {
            // Determine which answer type is being used
            bool hasTextAnswer = !string.IsNullOrEmpty(questionModel.TextAnswer);
            bool hasDateAnswer = questionModel.DateAnswer.HasValue;
            bool hasDropdownAnswer = questionModel.SelectedOptionId.HasValue;
            bool hasNumberAnswer = questionModel.NumberAnswer.HasValue;

            // Validate answer type based on the question's expected answer type
            switch (question.AnswerType)
            {
                case AnswerTypeEnum.Text:
                    if (!hasTextAnswer)
                    {
                        throw new CustomException($"Text answer is required for QuestionId {question.Id}.");
                    }
                    if (hasDateAnswer || hasDropdownAnswer || hasNumberAnswer)
                    {
                        throw new CustomException($"Only Text answer is allowed for QuestionId {question.Id}. Please provide only one.");
                    }
                    break;

                case AnswerTypeEnum.DateTime:
                    if (!hasDateAnswer)
                    {
                        throw new CustomException($"Date answer is required for QuestionId {question.Id}.");
                    }
                    if (hasTextAnswer || hasDropdownAnswer || hasNumberAnswer)
                    {
                        throw new CustomException($"Only DateTime answer is allowed for QuestionId {question.Id}. Please provide only one.");
                    }
                    break;

                case AnswerTypeEnum.Dropdown:
                    if (!hasDropdownAnswer)
                    {
                        throw new CustomException($"Dropdown answer is required for QuestionId {question.Id}.");
                    }
                    if (hasTextAnswer || hasDateAnswer || hasNumberAnswer)
                    {
                        throw new CustomException($"Only Dropdown answer is allowed for QuestionId {question.Id}. Please provide only one.");
                    }
                    break;

                case AnswerTypeEnum.Number:
                    if (!hasNumberAnswer)
                    {
                        throw new CustomException($"Number answer is required for QuestionId {question.Id}.");
                    }
                    if (hasTextAnswer || hasDateAnswer || hasDropdownAnswer)
                    {
                        throw new CustomException($"Only Number answer is allowed for QuestionId {question.Id}. Please provide only one.");
                    }
                    break;

                default:
                    throw new CustomException($"Unknown answer type for QuestionId {question.Id}.");
            }
        }
    }
}
