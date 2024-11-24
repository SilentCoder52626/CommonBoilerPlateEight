using System.ComponentModel.DataAnnotations;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class CancelBookingRequestModel
    {
        [Required]
        public string Reason { get; set; }
    }
}
