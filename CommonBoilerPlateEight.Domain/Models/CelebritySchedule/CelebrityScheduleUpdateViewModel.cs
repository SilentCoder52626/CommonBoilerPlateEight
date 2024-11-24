using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class CelebrityScheduleUpdateViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Date is required")]
        public DateOnly Date { get; set; }
        [Required(ErrorMessage = "From Time is required")]
        public TimeOnly FromTime { get; set; }
        [Required(ErrorMessage = "ToTime is required")]
        public TimeOnly ToTime { get; set; }

    }
}
