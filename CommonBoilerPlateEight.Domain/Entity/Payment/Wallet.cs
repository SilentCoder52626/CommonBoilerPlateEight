namespace CommonBoilerPlateEight.Domain.Entity
{
    public class Wallet : BaseEntity
    {
        public int CustomerId { get; set; }
        public decimal Balance { get; set; }

        public Customer Customer { get; set; }
    }

}
