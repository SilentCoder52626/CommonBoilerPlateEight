using CommonBoilerPlateEight.Domain.Enums;

namespace CommonBoilerPlateEight.Domain.Entity
{
    public class QuestionSetting : BaseEntity
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public AnswerTypeEnum AnswerType { get; set; }
        public DeliveryTypeEnum DeliveryType { get; set; }
        public bool IsActive { get; set; }

        public ICollection<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();

        public void AddQuestionChoice(string text)
        {
            AnswerOptions.Add(new AnswerOption(this, text));
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
