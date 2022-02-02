﻿using _360o.Server.API.V1.Stores.Model;

namespace _360o.Server.API.V1.Stores.DTOs
{
    public readonly record struct CreateItemRequest(
        string? EnglishName,
        string? EnglishDescription,
        string? FrenchName,
        string? FrenchDescription,
        MoneyValue? Price
        )
    {
    }
}
