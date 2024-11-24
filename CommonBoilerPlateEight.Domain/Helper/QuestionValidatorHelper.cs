using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Enums;
using CommonBoilerPlateEight.Domain.Exceptions;

namespace CommonBoilerPlateEight.Domain.Helper
{
    public static class QuestionValidatorHelper
    {
        public static void ValidateQuestionByDeliveryType(QuestionSetting questionSetting, DeliveryTypeEnum deliveryType)
        {
            if (questionSetting.DeliveryType != deliveryType)
            {
                throw new CustomException($"Question ID {questionSetting.Id} does not match the specified delivery type: {deliveryType.ToString().ToLower()}.");
            }
        }
    }

}
