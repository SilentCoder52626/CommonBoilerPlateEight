using CommonBoilerPlateEight.Domain.Enums;
using CommonBoilerPlateEight.Domain.Helper;

namespace CommonBoilerPlateEight.Domain.Entity
{
    public class Customer : BaseEntity
    {
        public string? FullName { get; set; }
        public string Email { get; set; }
        public int? CountryId { get; set; }
        public string? MobileNumber { get; set; }
        public string? ProfilePictureURL { get; set; }
        public AuthenticationTypeEnum AuthenticationType { get; set; }
        public StatusTypeEnum Status { get; set; }
        public GenderTypeEnum? Gender { get; set; }
        public bool IsCreatedByAdmin { get; set; }
        public string? ApprovedById { get; set; }
        public string? RejectedById { get; set; }
        public string? Description { get; set; }
        public Country? Country { get; set; }
        public string? DeviceId { get; set; }
        public string? OTP { get; set; }
        public string? Password { get; set; }
        public string? RejectionRemarks { get; set; }
        public bool IsOnline { get; set; }
        public bool IsActive { get; set; }
        public DateTime? OTPCreatedOn { get; set; }
        public ApplicationUser? CreatedByUser { get; set; }
        public ApplicationUser? ApprovedByUser { get; set; }
        public ApplicationUser? RejectedByUser { get; set; }
        public ICollection<CustomerToCelebrityType> CustomerToCelebrityTypes { get; set; } = new List<CustomerToCelebrityType>();
        public ICollection<CelebrityAdvertisement> CelebrityAdvertisements { get; set; } = new List<CelebrityAdvertisement>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();

        public void AddCelebrityTypes(CelebrityType type)
        {
            CustomerToCelebrityTypes.Add(new CustomerToCelebrityType(this, type));
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

        public void MarkAsApproved(ApplicationUser? user)
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
