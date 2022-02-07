namespace _360o.Server.Api.V1
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; }
        public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; protected set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? DeletedAt { get; private set; }

        public void SetUpdated()
        {
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void SetDelete()
        {
            DeletedAt = DateTimeOffset.UtcNow;
        }
    }
}