namespace _360o.Server.API.V1.Merchants.Model
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; }

        public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset UpdatedAt { get; protected set; } = DateTimeOffset.UtcNow;
    }
}

