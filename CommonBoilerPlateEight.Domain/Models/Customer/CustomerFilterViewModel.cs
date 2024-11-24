using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class CustomerFilterViewModel : PagedListBaseFilterModel
    {
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public string? Status { get; set; }
        public List<int> CelebrityTypes { get; set; } = new List<int>();
    }
}
