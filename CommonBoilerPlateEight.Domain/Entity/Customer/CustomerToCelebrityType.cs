namespace CommonBoilerPlateEight.Domain.Entity
{
    public class CustomerToCelebrityType :BaseEntity
    {
        protected CustomerToCelebrityType()
        {

        }
        public CustomerToCelebrityType(Customer customer, CelebrityType celebrityType)
        {
            CelebrityType = celebrityType;
            Customer = customer;
        }
        public int CustomerId { get; set; }
        public int CelebrityTypeId { get; set; }
        public CelebrityType CelebrityType { get; set; }
        public Customer Customer { get; set; }
    }
}
