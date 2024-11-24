namespace CommonBoilerPlateEight.Domain.Models
{
    public class PagedListBaseFilterModel
    {
        public int PageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 10;
    }
}
