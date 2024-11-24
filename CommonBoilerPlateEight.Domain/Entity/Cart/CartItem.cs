using CommonBoilerPlateEight.Domain.Enums;

namespace CommonBoilerPlateEight.Domain.Entity
{
    public class CartItem : BaseEntity
    {
        public int CustomerId { get; set; }
        public int CelebrityId { get; set; }
        public int CompanyTypeId { get; set; }
        public int CelebrityScheduleId { get; set; }
        public int CountryId { get; set; }
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        public string ManagerPhone { get; set; }
        public DateTime AdDate { get; set; }
        public decimal AdPrice { get; set; }
        public DeliveryTypeEnum DeliveryType { get; set; }

        // Relationships
        public Customer Customer { get; set; }
        public Celebrity Celebrity { get; set; }
        public CelebritySchedule CelebritySchedule { get; set; }
        public Country Country { get; set; }
        public CompanyType CompanyType { get; set; }

        public ICollection<CartItemQuestion> CartItemQuestions { get; set; } = new List<CartItemQuestion>();
    }

}
