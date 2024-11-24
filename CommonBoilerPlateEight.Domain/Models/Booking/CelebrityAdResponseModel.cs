namespace CommonBoilerPlateEight.Domain.Models
{
    public class CelebrityAdResponseModel
    {
        public int AdvertisementId { get; set; }
        public int CelebrityScheduleId { get; set; }
        public int CompanyTypeId { get; set; }
        public int CountryId { get; set; }
        public string? CompanyName { get; set; }
        public string? ContactPerson { get; set; }
        public string? ManagerPhone { get; set; }
        public string? Status { get; set; }
        public string? DeliveryType { get; set; }

        public DateTime AdDate { get; set; }
        public decimal AdPrice { get; set; }

        public ICollection<CelebrityAdQuestionResponseModel> Questions { get; set; }


    }
}
