using CommonBoilerPlateEight.Domain.Enums;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class CelebrityAdvertismentResponseModel
    {
        public int OrderId { get; set; }
        public string TrackingId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime BookingDate { get; set; }
        public string CelebrityName { get; set; }
        public decimal Total { get; set; }
        public int BookingId { get; set; }
        public string Status { get; set; }
        public string CompanyName { get; set; }
    }
}
