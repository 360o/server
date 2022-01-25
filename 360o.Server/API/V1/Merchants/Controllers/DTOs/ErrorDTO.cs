namespace _360o.Server.API.V1.Merchants.Controllers.DTOs
{
    public record struct ErrorDTO
    {
        public ErrorCode Code { get; set; }

        public string Message { get; set; }
    }

    public enum ErrorCode
    {
        ItemNotFound,
        NameAlreadyExists
    }
}

