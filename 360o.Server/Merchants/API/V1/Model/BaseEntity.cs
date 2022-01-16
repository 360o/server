using System;
namespace _360o.Server.Merchants.API.V1.Model
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; }

        public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset UpdatedAt { get; protected set; } = DateTimeOffset.UtcNow;
    }
}

