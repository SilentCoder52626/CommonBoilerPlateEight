using CommonBoilerPlateEight.Domain.Enums;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class AdvertisementFilterViewModel : PagedListBaseFilterModel
    {

        public BookingStatusEnum? Status { get; set; }

    }

}
