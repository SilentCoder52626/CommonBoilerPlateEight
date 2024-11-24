using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class GridResponseViewModel
    {
        public int TotalCount { get; set; }
        public object Data { get; set; }
    }
}
