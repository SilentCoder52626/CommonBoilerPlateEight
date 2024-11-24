using System.ComponentModel.DataAnnotations;
namespace CommonBoilerPlateEight.Domain.Models
{
    public class CelebrityValidateOtpViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "OTP Code is required")]
        public string OtpCode { get; set; }
    }
}
