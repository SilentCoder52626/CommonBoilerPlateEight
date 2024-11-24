using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class CelebrityLocationCreateViewModel
    {
        [Required(ErrorMessage = "Full Address is required")]
        public string FullAddress { get; set; }
        [Required(ErrorMessage = "Celebrity is required")]
        public int CelebrityId { get; set; }
        [Required(ErrorMessage = "Latitude is required")]
        public decimal Latitude { get; set; }
        [Required(ErrorMessage = "Longitude is required")]
        public decimal Longitude { get; set; }
        public string? Area { get; set; }
        public string? Block { get; set; }
        public string? Street { get; set; }
        public string? Governorate { get; set; }
        public string? GooglePlusCode { get; set; }
        public string? Note { get; set; }
        
    }
}
