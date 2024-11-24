namespace CommonBoilerPlateEight.Domain.Models
{
    public class WalletDetailViewModel
    {
        public int WalletId { get; set; }
        public int CustomerId { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
