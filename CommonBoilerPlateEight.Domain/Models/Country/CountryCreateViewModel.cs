using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class CountryCreateViewModel
    {
        public string Name { get; set; }
        public string FlagCode { get; set; }
        public string Code { get; set; }
        public string DialCode { get; set; }
    }
}
