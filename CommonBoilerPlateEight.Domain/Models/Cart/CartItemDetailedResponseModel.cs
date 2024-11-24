namespace CommonBoilerPlateEight.Domain.Models.Cart
{
    public class CartItemDetailedResponseModel
    {
        public int CartItemId { get; set; }
        public string? CelebrityName { get; set; }
        public string? CompanyName { get; set; }
        public string? ContactPerson { get; set; }
        public string? ManagerPhone { get; set; }
        public string? DeliveryType { get; set; }
        public DateTime AdDate { get; set; }
        public decimal AdPrice { get; set; }

        public ICollection<CartItemUpdateResponseModel> Questions { get; set; }


    }
}
