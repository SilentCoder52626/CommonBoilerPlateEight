namespace CommonBoilerPlateEight.Domain.Entity
{
    public class CelebrityType : BaseEntity
    {
        protected CelebrityType()
        {
            
        }
        public CelebrityType(ApplicationUser user, string name)
        {
            CreatedByUser = user;
            Name = name;
            Activate();
        }

        public void Update(string name)
        {
            Name = name;
            UpdatedDate = DateTime.Now;
        }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public ApplicationUser CreatedByUser { get; set; }

        public void Activate()
        {
            IsActive = true;
        }
        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
