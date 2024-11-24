using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class CompanyTypeCreateViewModel
    {
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }
    }
}
