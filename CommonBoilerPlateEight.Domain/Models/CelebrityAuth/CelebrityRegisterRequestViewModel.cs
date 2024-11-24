using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class CelebrityRegisterRequestViewModel
    {
        [Required(ErrorMessage = "Full Name is required.")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Country is required.")]
        public int CountryId { get; set; }
        [Required(ErrorMessage = "PhoneNumber is required.")]
        [RegularExpression(@"^\d{8,15}$", ErrorMessage = "Enter Phone Number with 8 to 15 digits.")]
        public string MobileNumber { get; set; }
        public IFormFile? ProfileImageFile { get; set; }

        public DateTime? TimeToCall { get; set; }  // Date And Time For the Celebrity to call by admin
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password must match.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Price Per Ad is required")]
        [RegularExpression(@"^\d+(\.\d{1,3})?$", ErrorMessage = "Please enter a valid price.")]
        public decimal PricePerPost { get; set; }
        [Required(ErrorMessage = "Price Per Event is required")]
        [RegularExpression(@"^\d+(\.\d{1,3})?$", ErrorMessage = "Please enter a valid price.")]
        public decimal PricePerEvent { get; set; }
        [Required(ErrorMessage = "Price Per Delivery is required")]
        [RegularExpression(@"^\d+(\.\d{1,3})?$", ErrorMessage = "Please enter a valid price.")]
        public decimal PricePerDelivery { get; set; }
        [Required(ErrorMessage = "Celebrity Type is required")]
        public int CelebrityTypeId { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "DeviceId is required")]
        public string DeviceId { get; set; }
        public string? Description { get; set; }
    }
}
