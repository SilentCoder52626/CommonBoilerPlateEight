
using System.ComponentModel.DataAnnotations;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class EmailSetupViewModel
    {
        [Required(ErrorMessage = "Host Server is required.")]
        public string? Host { get; set; }
        [RegularExpression(@"^\d+$", ErrorMessage = "Port must be a number.")]
        [Required(ErrorMessage = "Port is required.")]
        public string? Port { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "From Name is required.")]
        public string? FromName { get; set; }
        [Required(ErrorMessage = "From Email is required.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
        public string? FromEmail { get; set; }
    }
}
