namespace CommonBoilerPlateEight.Domain.Entity
{
    public class BaseEntity<TKey>
    {
        public TKey Id { get; set; } = default!;
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public void MarkAsDeleted()
        {
            DeletedDate = DateTime.Now;
        }
    }

    public class BaseEntity : BaseEntity<int>
    {
    }

}
