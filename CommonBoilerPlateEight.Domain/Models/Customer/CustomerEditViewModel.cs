using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class CustomerEditViewModel
    {
        public int Id { get; set; }
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
        public string? ProfileImage { get; set; }
        [Required(ErrorMessage = "Interests is required.")]
        public List<int> CelebrityTypeIds { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }
        public string? Description { get; set; }
    }
}
