using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Enums;
using CommonBoilerPlateEight.Domain.Exceptions;

namespace CommonBoilerPlateEight.Domain.Helper
{
    public static class DetermineAdPriceHelper
    {
        public static decimal DetermineAdPrice(DeliveryTypeEnum deliveryType, Celebrity celebrity)
        {
            return deliveryType switch
            {
                DeliveryTypeEnum.Delivery => celebrity.PricePerDelivery,
                DeliveryTypeEnum.Post => celebrity.PricePerPost,
                DeliveryTypeEnum.Event => celebrity.PricePerEvent,
                _ => throw new CustomException("Invalid delivery type specified")
            };
        }
    }
}
