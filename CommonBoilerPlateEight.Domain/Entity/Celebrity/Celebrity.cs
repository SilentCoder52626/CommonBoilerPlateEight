using CommonBoilerPlateEight.Domain.Enums;
using CommonBoilerPlateEight.Domain.Helper;

namespace CommonBoilerPlateEight.Domain.Entity
{
    public class Celebrity : BaseEntity
    {

        public string? FullName { get; set; }
        public string Email { get; set; }
        public int? CountryId { get; set; }
        public AuthenticationTypeEnum AuthenticationType { get; set; }
        public string? MobileNumber { get; set; }
        public string? ProfilePictureURL { get; set; }
        public DateTime? TimeToCall { get; set; }  // Date And Time For the Celebrity to call by admin
        public string? Password { get; set; }
        public decimal PricePerPost { get; set; }
        public decimal PricePerEvent { get; set; }
        public decimal PricePerDelivery { get; set; }
        public string PriceRange { get; set; }
        public StatusTypeEnum Status { get; set; }
        public GenderTypeEnum? Gender { get; set; }
        public bool IsCreatedByAdmin { get; set; }
        public string? ApprovedById { get; set; }
        public string? RejectedById { get; set; }
        public string? Description { get; set; }
        public Country? Country { get; set; }
        public string? DeviceId { get; set; }
        public string? OTP { get; set; }
        public string? RejectionRemarks { get; set; }
        public bool IsOnline { get; set; }
        public bool IsActive { get; set; }
        public DateTime? OTPCreatedOn { get; set; }
        public ApplicationUser? CreatedByUser { get; set; }
        public ApplicationUser? ApprovedByUser { get; set; }
        public ApplicationUser? RejectedByUser { get; set; }
        public ICollection<CelebrityToType> CelebrityToTypes { get; set; } = new List<CelebrityToType>();
        public ICollection<CelebrityToSocialLink> CelebrityToSocialLinks { get; set; } = new List<CelebrityToSocialLink>();
        public ICollection<CelebrityToAttachment> CelebrityToAttachments { get; set; } = new List<CelebrityToAttachment>();
        public ICollection<CelebritySchedule> CelebritySchedules { get; set; } = new List<CelebritySchedule>();
        public ICollection<CelebrityLocation> CelebrityLocations { get; set; } = new List<CelebrityLocation>();
        public ICollection<CelebrityAdvertisement> CelebrityAdvertisements { get; set; } = new List<CelebrityAdvertisement>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public void AddCelebrityTypes(CelebrityType type)
        {
            CelebrityToTypes.Add(new CelebrityToType(this, type));
        }
        public void SetPriceRange()
        {
            decimal[] prices = new decimal[] { PricePerPost, PricePerEvent, PricePerDelivery };
            Array.Sort(prices);
            string minPrice = prices[0] % 1 == 0 ? prices[0].ToString("0") : prices[0].ToString("0.000");
            string maxPrice = prices[2] % 1 == 0 ? prices[2].ToString("0") : prices[2].ToString("0.000");
            PriceRange = $"{minPrice} - {maxPrice}";
        }

        public void AddOrUpdateSocialLink(SocialLinkEnum platform, string url)
        {
            var existingLink = CelebrityToSocialLinks.FirstOrDefault(x => x.Platform == platform);

            if (existingLink != null)
            {
                existingLink.Update(url);
            }
            else
            {
                var newLink = new CelebrityToSocialLink(this, platform, url);
                CelebrityToSocialLinks.Add(newLink);
            }
        }

        public void AddCelebrityAttachment(CelebrityAttachmentTypeEnum type, string filePath, string fileName, string fileContent)
        {
            CelebrityToAttachments.Add(new CelebrityToAttachment(this, filePath, fileName, type, fileContent));
        }
        public void SetProfilePicture(string imageUrl)
        {
            ProfilePictureURL = imageUrl;
        }
        public void SetPassword(string password)
        {
            Password = password;
        }
        public void SetCreatedBy(ApplicationUser user)
        {
            CreatedByUser = user;
            IsCreatedByAdmin = true;
        }

        public void MarkAsApproved(ApplicationUser user)
        {
            ApprovedByUser = user;
            Status = StatusTypeEnum.Approved;
            RejectionRemarks = null;
        }
        public void MarkAsRejected(string rejectionRemarks, ApplicationUser rejectedBy)
        {
            Status = StatusTypeEnum.Rejected;
            RejectionRemarks = rejectionRemarks;
            RejectedByUser = rejectedBy;
        }

        public void MarkAsPending()
        {
            Status = StatusTypeEnum.Pending;
        }

        public void SetOtp()
        {
            OTP = RandomStringGenerator.GenerateOtp();
            OTPCreatedOn = DateTime.Now;
        }

        public void ResetOtp()
        {
            OTP = null;
            OTPCreatedOn = null;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
