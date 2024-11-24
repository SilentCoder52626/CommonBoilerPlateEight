using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class TokenModel
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
    }
}
