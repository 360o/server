﻿using _360o.Server.Api.V1.Stores.Model;

namespace _360o.Server.Api.V1.Stores.DTOs
{
    public readonly record struct UpdateItemRequest(
        string? EnglishName,
        string? EnglishDescription,
        string? FrenchName,
        string? FrenchDescription,
        MoneyValue? Price
        )
    {
    }
}