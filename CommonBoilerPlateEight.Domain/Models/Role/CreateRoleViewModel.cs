
using System.ComponentModel.DataAnnotations;


namespace CommonBoilerPlateEight.Domain.Models
{
    public class CreateRoleViewModel
    { 
            [Required]
            public string Name { get; set; }        
    }
}
