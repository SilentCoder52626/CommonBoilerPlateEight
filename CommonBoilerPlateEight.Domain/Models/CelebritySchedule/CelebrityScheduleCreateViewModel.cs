using System.ComponentModel.DataAnnotations;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class CelebrityScheduleCreateViewModel
    {
        [Required(ErrorMessage ="Celebrity is required")]
        public int CelebrityId { get; set; }
        [Required(ErrorMessage = "Date is required")]
        public DateOnly Date { get; set; }
        [Required(ErrorMessage = "From Time is required")]
        public TimeOnly FromTime { get; set; }
        [Required(ErrorMessage = "ToTime is required")]
        public TimeOnly ToTime { get; set; }
    }
}
