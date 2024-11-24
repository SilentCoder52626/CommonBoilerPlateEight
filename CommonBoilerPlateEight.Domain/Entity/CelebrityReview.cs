namespace CommonBoilerPlateEight.Domain.Entity
{
    public class CelebrityReview : BaseEntity
    {
        public int AdId { get; set; }
        public decimal Rating { get; set; }
        public string? ReviewText { get; set; }
        public bool IsApprovedByAdmin { get; set; }
        public string? ApprovedById { get; set; }

        // Navigation Properties
        public ApplicationUser? ApprovedBy { get; set; }
        public CelebrityAdvertisement CelebrityAdvertisement { get; set; }
        public void MarkAsApproved(ApplicationUser user)
        {
            ApprovedBy = user;

        }

    }

}
