namespace CommonBoilerPlateEight.Domain.Entity
{
    public class CelebritySchedule : BaseEntity
    {
        public DateOnly Date { get; set; }
        public TimeOnly From { get; set; }
        public TimeOnly To { get; set; }
        public bool IsActive { get; set; }
        public int CelebrityId { get; set; }
        public Celebrity Celebrity { get; set; }
        public ApplicationUser? CreatedByUser { get; set; }

        public string GetSchedule()
        {
            return From.ToString("hh:mm tt") + " - " + To.ToString("hh:mm tt");
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void SetCreatedBy(ApplicationUser user)
        {
            CreatedByUser = user;

        }
    }
}
