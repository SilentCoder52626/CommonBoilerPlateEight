using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class CelebrityScheduleFilterViewModel : PagedListBaseFilterModel
    {
        public int CelebrityId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
