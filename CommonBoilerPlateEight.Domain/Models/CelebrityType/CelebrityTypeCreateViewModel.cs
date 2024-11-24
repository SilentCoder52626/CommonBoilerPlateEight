using System.ComponentModel.DataAnnotations;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class CelebrityTypeCreateViewModel
    {
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }
    }
}
