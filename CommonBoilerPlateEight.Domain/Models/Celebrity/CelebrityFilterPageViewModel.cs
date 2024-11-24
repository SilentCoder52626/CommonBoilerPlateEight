namespace CommonBoilerPlateEight.Domain.Models.Celebrity
{
    public class CelebrityFilterPageViewModel : PagedListBaseFilterModel
    {
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public List<int> CelebrityTypes { get; set; } = new List<int>();
        public decimal FromPrice { get; set; }
        public decimal ToPrice { get; set; }

        public decimal Rating { get; set; }


    }
}
