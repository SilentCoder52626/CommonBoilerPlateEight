namespace CommonBoilerPlateEight.Domain.Models
{
    public class CelebrityAdAnswerDetailModel
    {
        public int? SelectedOptionId { get; set; }
        public string TextAnswer { get; set; }
        public DateTime? DateAnswer { get; set; }
        public decimal? NumberAnswer { get; set; }
        public string? AnswerOptionText { get; set; }
    }
}
