namespace CommonBoilerPlateEight.Domain.Models
{
    public class ReviewResponseModel
    {
        public int AdId { get; set; }
        public int ReviewId { get; set; }
        public decimal Rating { get; set; }
        public string? ReviewText { get; set; }
        public string CustomerName { get; set; }

    }

}
