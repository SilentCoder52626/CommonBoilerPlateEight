using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class CustomerEditTypesViewModel
    {
        public int CustomerId { get; set; }
        public List<int> CelebrityTyesIds { get; set; } = new List<int>();
    }
}
