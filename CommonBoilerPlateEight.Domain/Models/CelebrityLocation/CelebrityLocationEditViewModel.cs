using System.ComponentModel.DataAnnotations;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class CelebrityLocationEditViewModel
    {
        public int Id { get; set; }
        public int CelebrityId { get; set; }
        [Required(ErrorMessage = "Full Address is required")]
        public string FullAddress { get; set; }

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
