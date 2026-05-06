namespace VisitTracking.Domain.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public void Touch()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
