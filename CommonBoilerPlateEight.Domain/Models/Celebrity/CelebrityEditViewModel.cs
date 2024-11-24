using Microsoft.AspNetCore.Http;
using CommonBoilerPlateEight.Domain.Models.Celebrity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class CelebrityEditViewModel
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
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }
        public string? ProfileImage { get; set; }
        public IFormFile? ProfileImageFile { get; set; }
        public DateTime? TimeToCall { get; set; }
        [Required(ErrorMessage = "Price Per Ad is required")]
        [RegularExpression(@"^\d+(\.\d{1,3})?$", ErrorMessage = "Please enter a valid price.")]
        public decimal PricePerPost { get; set; }
        [Required(ErrorMessage = "Price Per Event is required")]
        [RegularExpression(@"^\d+(\.\d{1,3})?$", ErrorMessage = "Please enter a valid price.")]
        public decimal PricePerEvent { get; set; }
        [Required(ErrorMessage = "Price Per Delivery is required")]
        [RegularExpression(@"^\d+(\.\d{1,3})?$", ErrorMessage = "Please enter a valid price.")]
        public decimal PricePerDelivery { get; set; }
        public string PriceRange { get; set; }
        [Required(ErrorMessage = "Celebrity Type is required")]
        public int CelebrityTypeId { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; }
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

        public CelebrityAttachmentViewModel CivilIdAttachment { get; set; } = new CelebrityAttachmentViewModel();
        public CelebrityAttachmentViewModel ContractAttachment { get; set; } = new CelebrityAttachmentViewModel();
    }
}
