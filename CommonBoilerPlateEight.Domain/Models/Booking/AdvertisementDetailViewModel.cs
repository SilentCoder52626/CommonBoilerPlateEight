namespace CommonBoilerPlateEight.Domain.Models
{
    public class AdvertisementDetailViewModel
    {
        public int AdvertisementId { get; set; }
        public string TrackingId { get; set; }
        public string? CelebrityName { get; set; }
        public string Status { get; set; }
        public string? CompanyName { get; set; }
        public string? ContactPerson { get; set; }
        public string? ManagerPhone { get; set; }
        public string? DeliveryType { get; set; }
        public DateTime AdDate { get; set; }
        public decimal AdPrice { get; set; }
    }
}

