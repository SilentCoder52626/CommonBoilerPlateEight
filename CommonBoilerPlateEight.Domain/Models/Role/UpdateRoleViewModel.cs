
using System.ComponentModel.DataAnnotations;


namespace CommonBoilerPlateEight.Domain.Models
{
    public class UpdateRoleViewModel
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
