namespace CommonBoilerPlateEight.Domain.Models
{
    public class BookingResponseModel
    {
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }

}
