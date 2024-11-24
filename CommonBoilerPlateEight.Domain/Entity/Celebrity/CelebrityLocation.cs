using NetTopologySuite.Geometries;

namespace CommonBoilerPlateEight.Domain.Entity
{
    public class CelebrityLocation : BaseEntity
    {
        public string FullAddress { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string? Area { get; set; }
        public string? Block { get; set; }
        public string? Street { get; set; }
        public string? Governorate { get; set; }
        public string? GooglePlusCode { get; set; }
        public string? Note { get; set; }
        public Point Location { get; set; }
        public int CelebrityId { get; set; }
        public Celebrity Celebrity { get; set; }
        public void SetLocationDetails(decimal latitude, decimal longitude, string fullAddress,string? note, string? area, string? block, string? street, string? governorate, string? plusCode)
        {
            Latitude = latitude;
            Longitude = longitude;
            Area = area;
            Block = block;
            Street = street;
            Governorate = governorate;
            GooglePlusCode = plusCode;
            FullAddress = fullAddress;
            Note= note;
        }
    }
}
