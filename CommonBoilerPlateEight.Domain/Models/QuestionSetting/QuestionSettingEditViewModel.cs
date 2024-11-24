using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class QuestionSettingEditViewModel
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string AnswerType { get; set; }
        public string DeliveryType { get; set; }
        public List<AnswerOptionViewModel> AnswerOptions { get; set; } = new List<AnswerOptionViewModel>();
    }
}
