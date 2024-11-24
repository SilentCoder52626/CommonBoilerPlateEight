using CommonBoilerPlateEight.Domain.Enums;

namespace CommonBoilerPlateEight.Domain.Entity
{
    public class BookingHistory : BaseEntity
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int BookingId { get; set; }
        public BookingStatusEnum Status { get; set; }
        // Navigation Property
        public Booking Booking { get; set; }


        protected BookingHistory()
        {

        }

        public BookingHistory(Booking booking, string comment, BookingStatusEnum status)
        {
            Booking = booking;
            Comment = comment;
            Status = status;
        }
    }

}
