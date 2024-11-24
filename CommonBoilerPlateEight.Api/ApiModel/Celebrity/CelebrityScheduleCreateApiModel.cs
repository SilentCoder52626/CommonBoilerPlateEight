using System.ComponentModel.DataAnnotations;

namespace CommonBoilerPlateEight.Api.ApiModel
{
    public class CelebrityScheduleCreateApiModel
    {
        [Required(ErrorMessage = "Date is required")]
        public DateOnly Date { get; set; }
        [Required(ErrorMessage = "From Time is required")]
        public TimeOnly FromTime { get; set; }
        [Required(ErrorMessage = "ToTime is required")]
        public TimeOnly ToTime { get; set; }
    }
}
