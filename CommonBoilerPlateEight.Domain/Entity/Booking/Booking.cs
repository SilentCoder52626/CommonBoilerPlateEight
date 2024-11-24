using CommonBoilerPlateEight.Domain.Enums;

namespace CommonBoilerPlateEight.Domain.Entity
{

    public class Booking : BaseEntity
    {
        public BookingStatusEnum Status { get; set; }
        public string TrackingId { get; set; }
        public int CustomerId { get; set; }
        public ApplicationUser? CreatedByUser { get; set; }
        public bool IsCreatedByAdmin { get; set; }
        // Relationships
        public ICollection<CelebrityAdvertisement> CelebrityAdvertisements { get; set; } = new List<CelebrityAdvertisement>();
        public ICollection<BookingHistory> BookingHistories { get; set; } = new List<BookingHistory>();
        public Customer Customer { get; set; }



        public void SetCreatedBy(ApplicationUser user)
        {
            CreatedByUser = user;
            IsCreatedByAdmin = true;
        }

        public void AddBookingHistory(string comment, BookingStatusEnum status)
        {
            BookingHistories.Add(new BookingHistory(this, comment, status));
        }

    }
}
