using System.ComponentModel.DataAnnotations;

namespace CommonBoilerPlateEight.Web.ViewModel
{
    public class CelebrityIndexFilterViewModel
    {
        public int start { get; set; }
        public int length { get; set; }
        public int draw { get; set; }
        public string? Search { get; set; }
    }
}
