namespace CommonBoilerPlateEight.Domain.Models
{
    public class ReviewDetailViewModel
    {
        public int Id { get; set; }
        public string CelebrityName { get; set; }
        public decimal Rating { get; set; }
        public string ReviewText { get; set; }
        public string CustomerName { get; set; }
        public bool IsApprovedByAdmin { get; set; }
        public List<ReviewResponseModel> Reviews { get; set; }
    }
}
