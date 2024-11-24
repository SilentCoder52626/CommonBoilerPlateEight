using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class CelebrityScheduleViewModel
    {
        public int Id { get; set; }
        public int CelebrityId { get; set; }
        public string Celebrity { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly FromTime { get; set; }
        public TimeOnly ToTime { get; set; }
        public bool IsActive { get; set; }
        public string FormattedSchedule { get; set; }
    }
}
