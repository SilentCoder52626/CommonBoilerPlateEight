using CommonBoilerPlateEight.Domain.Enums;

namespace CommonBoilerPlateEight.Domain.Entity
{
    public class CelebrityAdvertisement : BaseEntity
    {
        public string TrackingId { get; set; }
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public int CelebrityId { get; set; }
        public int CelebrityScheduleId { get; set; }
        public int CompanyTypeId { get; set; }
        public int CountryId { get; set; }
        public string? CompanyName { get; set; }
        public string? ContactPerson { get; set; }
        public string? ManagerPhone { get; set; }
        public decimal AdPrice { get; set; }

        public BookingStatusEnum Status { get; set; }
        public DeliveryTypeEnum DeliveryType { get; set; }
        public DateTime AdDate { get; set; }

        // Relationships
        public ICollection<CelebrityAdvertismentQuestion> CelebrityAdvertismentQuestions { get; set; } = new List<CelebrityAdvertismentQuestion>();
        public ICollection<CelebrityReview> CelebrityReviews { get; set; } = new List<CelebrityReview>();
        public ICollection<CelebrityAdHistory> CelebrityAdHistories { get; set; } = new List<CelebrityAdHistory>();
        public Customer Customer { get; set; }
        public Celebrity Celebrity { get; set; }
        public CelebritySchedule CelebritySchedule { get; set; }
        public Country Country { get; set; }
        public CompanyType CompanyType { get; set; }
        public Booking Booking { get; set; }


        public void AddAdvertismentHistory(string comment, BookingStatusEnum status)
        {
            CelebrityAdHistories.Add(new CelebrityAdHistory(this, comment, status));
        }
    }
}
