using Microsoft.AspNetCore.Identity;

namespace CommonBoilerPlateEight.Domain.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public void UnBlockUser()
        {
            AccessFailedCount = 0;
            LockoutEnd = DateTime.Now.AddDays(-1);
        }

        public void BlockUser()
        {
            AccessFailedCount = 1000;
            LockoutEnd = DateTime.Now.AddYears(4);
        }
    }
}
