namespace CommonBoilerPlateEight.Domain.Models.Cart
{
    public class CartItemUpdateResponseModel
    {

        public int QuestionId { get; set; }
        public int? SelectedOptionId { get; set; }
        public string? TextAnswer { get; set; }
        public DateTime? DateAnswer { get; set; }
        public decimal? NumberAnswer { get; set; }

    }
}
