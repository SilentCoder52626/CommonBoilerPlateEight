using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class CustomerVerifyAccountViewModel
    {
        public string Email { get; set; }
        public string OTP { get; set; }
    }
}
