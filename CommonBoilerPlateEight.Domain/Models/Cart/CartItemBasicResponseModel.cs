namespace CommonBoilerPlateEight.Domain.Models
{
    public class CartItemBasicResponseModel
    {
        public int CartItemId { get; set; }
        public int CustomerId { get; set; }
        public int? CelebrityId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
