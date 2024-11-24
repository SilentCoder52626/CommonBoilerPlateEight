namespace CommonBoilerPlateEight.Domain.Entity
{
    public class AnswerOption : BaseEntity
    {
        protected AnswerOption()
        {
            
        }
        public AnswerOption(QuestionSetting questionSetting, string text)
        {
            QuestionSetting = questionSetting;
            OptionText = text;
        }
        public int QuestionSettingId { get; set; }
        public QuestionSetting QuestionSetting { get; set; }
        public string OptionText { get; set; }
    }
}
