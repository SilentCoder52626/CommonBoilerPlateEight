namespace CommonBoilerPlateEight.Domain.Models
{
    public class CelebrityBasicDetailViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string DialCode { get; set; }
        public string MobileNumber { get; set; }
        public string? ProfileImageURL { get; set; }
        public DateTime? TimeToCall { get; set; }
        public string Status { get; set; }
        public decimal PricePerAd { get; set; }
        public decimal PricePerEvent { get; set; }
        public decimal PricePerDelivery { get; set; }
        public string CelebrityType { get; set; }
        public decimal AverageRating { get; set; }
        public bool IsActive { get; set; }
    }
}
