namespace CommonBoilerPlateEight.Domain.Entity
{
    public class CelebrityAdvertismentQuestion : BaseEntity
    {
        public int AdId { get; set; }
        public int QuestionId { get; set; }
        public string? TextAnswer { get; set; }      // If AnswerType = Text
        public int? SelectedOptionId { get; set; }   // If AnswerType = Dropdown
        public DateTime? DateAnswer { get; set; }    // If AnswerType = Date
        public decimal? NumberAnswer { get; set; } // If AnswerType = Date

        // Relationships
        public CelebrityAdvertisement CelebrityAdvertisement { get; set; }
        public QuestionSetting QuestionSetting { get; set; }
    }
}
