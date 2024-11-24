namespace CommonBoilerPlateEight.Api.ApiModel
{
    public class CelebrityScheduleFilterApiModel
    {
        public int PageNumber { get; set; } = 1;
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
