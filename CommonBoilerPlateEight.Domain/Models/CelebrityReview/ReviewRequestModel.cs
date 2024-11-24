using System.ComponentModel.DataAnnotations;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class ReviewRequestModel
    {
        [Required(ErrorMessage = "AdId is required")]
        public int AdId { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public decimal Rating { get; set; }

        [MaxLength(1000, ErrorMessage = "ReviewText cannot exceed 1000 characters")]
        public string? ReviewText { get; set; }
    }
}
