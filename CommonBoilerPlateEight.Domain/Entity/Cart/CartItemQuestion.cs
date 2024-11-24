namespace CommonBoilerPlateEight.Domain.Entity
{
    public class CartItemQuestion : BaseEntity
    {
        public int CartItemId { get; set; }  // Link to CartItem
        public int QuestionId { get; set; }
        public string? TextAnswer { get; set; }
        public int? SelectedOptionId { get; set; }
        public DateTime? DateAnswer { get; set; }
        public decimal? NumberAnswer { get; set; }

        // Relationships
        public CartItem CartItem { get; set; }
        public QuestionSetting QuestionSetting { get; set; }
    }

}
