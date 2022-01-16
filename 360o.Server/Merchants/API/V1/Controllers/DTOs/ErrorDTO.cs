using System;
using System.Text.Json.Serialization;

namespace _360o.Server.Merchants.API.V1.Controllers.DTOs
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

