using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class CelebrityLocationViewModel
    {
        public int Id { get; set; }
        public string FullAddress { get; set; }
        public int CelebrityId { get; set; }
        public string Celebrity { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string? Area { get; set; }
        public string? Block { get; set; }
        public string? Street { get; set; }
        public string? Governorate { get; set; }
        public string? GooglePlusCode { get; set; }   
        public string? Note { get; set; }   
    }
}
