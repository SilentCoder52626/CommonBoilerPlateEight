using System.ComponentModel.DataAnnotations;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class CelebrityAdQuestionRequestModel
    {
        [Required(ErrorMessage = "QuestionId is required")]
        public int QuestionId { get; set; }

        public string TextAnswer { get; set; } = string.Empty;

        public int? SelectedOptionId { get; set; }

        public DateTime? DateAnswer { get; set; }

        public decimal? NumberAnswer { get; set; }
    }
}
