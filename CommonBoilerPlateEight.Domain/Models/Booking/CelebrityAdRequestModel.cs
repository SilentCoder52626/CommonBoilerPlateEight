using CommonBoilerPlateEight.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class CelebrityAdRequestModel
    {
        [Required(ErrorMessage = "CelebrityId is required")]
        public int CelebrityId { get; set; }

        [Required(ErrorMessage = "CelebrityScheduleId is required")]
        public int CelebrityScheduleId { get; set; }

        [Required(ErrorMessage = "CompanyTypeId is required")]
        public int CompanyTypeId { get; set; }

        [Required(ErrorMessage = "CountryId is required")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "CompanyName is required")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "ContactPerson is required")]
        [StringLength(400, ErrorMessage = "ContactPerson cannot exceed 400 characters")]
        public string ContactPerson { get; set; }

        [Required(ErrorMessage = "ManagerPhone is required.")]
        [RegularExpression(@"^\d{8,15}$", ErrorMessage = "Enter Phone Number with 8 to 15 digits.")]
        public string ManagerPhone { get; set; }

        [Required(ErrorMessage = "DeliveryType is required")]
        [EnumDataType(typeof(DeliveryTypeEnum), ErrorMessage = "Invalid DeliveryType")]
        public DeliveryTypeEnum DeliveryType { get; set; }

        // Questions for each ad
        public List<CelebrityAdQuestionRequestModel> Questions { get; set; }
    }
}
