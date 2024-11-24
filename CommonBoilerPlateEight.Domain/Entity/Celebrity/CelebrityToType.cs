namespace CommonBoilerPlateEight.Domain.Entity
{
    public class CelebrityToType : BaseEntity
    {
        protected CelebrityToType()
        {
            
        }
        public CelebrityToType(Celebrity celebrity, CelebrityType celebrityType)
        {
            CelebrityType = celebrityType;
            Celebrity = celebrity;
        }
        public int CelebrityId { get; set; }
        public int CelebrityTypeId { get; set; }
        public CelebrityType CelebrityType { get; set; }
        public Celebrity Celebrity { get; set; }
    }
}
