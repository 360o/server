﻿using System;
namespace _360o.Server.API.V1.Stores.Model
{
    public class Location
    {
        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; private set; }

        public double Longitude { get; private set; }
    }
}
