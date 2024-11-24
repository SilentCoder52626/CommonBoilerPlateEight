using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class CelebrityCreateViewModel
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
        public string? Description { get; set; }
        [RegularExpression(@"^(https?:\/\/)?(www\.)?facebook\.com\/[A-Za-z0-9._-]+\/?$", ErrorMessage = "Please enter a valid Facebook link.")]
        public string? FacebookLink { get; set; }

        [RegularExpression(@"^(https?:\/\/)?(www\.)?instagram\.com\/[A-Za-z0-9._-]+\/?$",
            ErrorMessage = "Please enter a valid Instagram link.")]
        public string? InstagramLink { get; set; }


        [RegularExpression(@"^(https?:\/\/)?(www\.)?snapchat\.com\/add\/[A-Za-z0-9._-]+\/?$",
            ErrorMessage = "Please enter a valid Snapchat link.")]
        public string? SnapchatLink { get; set; }


        [RegularExpression(@"^(https?:\/\/)?(www\.)?twitter\.com\/[A-Za-z0-9._-]+\/?$",
            ErrorMessage = "Please enter a valid Twitter link.")]
        public string? TwitterLink { get; set; }


        [RegularExpression(@"^(https?:\/\/)?(www\.)?threads\.net\/[A-Za-z0-9._-]+\/?$",
            ErrorMessage = "Please enter a valid Threads link.")]
        public string? ThreadsLink { get; set; }

        [RegularExpression(@"^(https?:\/\/)?(www\.)?(youtube\.com|youtu\.be)\/(watch\?v=[A-Za-z0-9._-]+|channel\/[A-Za-z0-9._-]+|c\/[A-Za-z0-9._-]+|user\/[A-Za-z0-9._-]+|@[\w.-]+)\/?$",
            ErrorMessage = "Please enter a valid YouTube link.")]
        public string? YoutubeLink { get; set; }
        public IFormFile? CivilIdFile { get; set; }
        public IFormFile? ContractFile { get; set; }
    }


}
