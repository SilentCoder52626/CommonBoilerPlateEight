namespace CommonBoilerPlateEight.Domain.Models
{
    public class CelebrityFilterViewModel :PagedListBaseFilterModel
    {
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public string? Status { get; set; }
        public List<int> CelebrityTypes { get; set; } = new List<int>();
        public decimal FromPrice { get; set; } 
        public decimal ToPrice { get; set; } 
    }
}
