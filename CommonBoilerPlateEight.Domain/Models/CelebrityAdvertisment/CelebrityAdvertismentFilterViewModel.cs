using CommonBoilerPlateEight.Domain.Enums;

namespace CommonBoilerPlateEight.Domain.Models.CelebrityAdvertisment
{
    public class CelebrityAdvertismentFilterViewModel : PagedListBaseFilterModel
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string? Status { get; set; }
        
    }
}
