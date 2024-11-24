using CommonBoilerPlateEight.Domain.Enums;

namespace CommonBoilerPlateEight.Domain.Entity
{
    public class CelebrityAdHistory : BaseEntity
    {
        protected CelebrityAdHistory()
        {

        }

        public CelebrityAdHistory(CelebrityAdvertisement advertisement, string comment, BookingStatusEnum status)
        {
            CelebrityAd = advertisement;
            Comment = comment;
            Status = status;
        }

        public int Id { get; set; }
        public string Comment { get; set; }
        public int AdId { get; set; }
        public BookingStatusEnum Status { get; set; }
        // Navigation Property
        public CelebrityAdvertisement CelebrityAd { get; set; }


    }

}
