using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class CustomerBasicDetailResponseViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        public string CountryDialCode { get; set; }
        public string MobileNumber { get; set; }    
        public string ProfileImageUrl { get; set; }
        public string Gender { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
    }
}
