using System.ComponentModel.DataAnnotations;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class QuestionSettingCreateViewModel
    {
        [Required(ErrorMessage = "Question is required.")]
        public string Question { get; set; }
        [Required(ErrorMessage = "Answer Type is required.")]
        public string AnswerType { get; set; }
        [Required(ErrorMessage = "Ad Type is required.")]
        public string DeliveryType { get; set; }
        public List<AnswerOptionViewModel> AnswerOptions { get; set; } = new List<AnswerOptionViewModel>();

    }


}
