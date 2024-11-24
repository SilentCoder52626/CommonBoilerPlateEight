using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class QuestionSettingFilterViewModel: PagedListBaseFilterModel
    {
        public string? AnswerType { get; set; }
        public string? DeliveryType { get; set; }
    }
}
