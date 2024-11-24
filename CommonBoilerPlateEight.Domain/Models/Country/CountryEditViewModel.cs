using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Models
{
    public  class CountryEditViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Flag Code is required.")]
        public string FlagCode { get; set; }
        [Required(ErrorMessage = "Code is required.")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string DialCode { get; set; }
    }
}
