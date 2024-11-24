namespace CommonBoilerPlateEight.Domain.Models
{
    public class UserResponseViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsBlocked { get; set; }
        public string RoleId { get; set; }
    }
}
