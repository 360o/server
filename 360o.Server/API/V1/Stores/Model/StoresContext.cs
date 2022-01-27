using System;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace _360o.Server.API.V1.Stores.Model
{
    public class StoresContext : DbContext
    {
        public DbSet<Store> Stores { get; set; }
        public DbSet<Place> Places { get; set; }

        public StoresContext(DbContextOptions<StoresContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("postgis");
            modelBuilder.Entity<Store>().Property(s => s.EnglishCategories).HasField("_englishCategories");
            modelBuilder.Entity<Store>().Property(s => s.FrenchCategories).HasField("_frenchCategories");
            modelBuilder
                .Entity<Store>()
                .HasGeneratedTsVectorColumn(s => s.EnglishSearchVector, "english", s => new { s.DisplayName, s.EnglishShortDescription, s.EnglishLongDescription })
                .HasIndex(s => s.EnglishSearchVector)
                .HasMethod("GIN");
            modelBuilder
                .Entity<Store>()
                .HasGeneratedTsVectorColumn(s => s.FrenchSearchVector, "french", s => new { s.DisplayName, s.FrenchShortDescription, s.FrenchLongDescription })
                .HasIndex(s => s.FrenchSearchVector)
                .HasMethod("GIN");

            modelBuilder.Entity<Place>().Ignore(p => p.Location);
            modelBuilder.Entity<Place>().Property(p => p.Point).HasColumnType("geography (point)");
            modelBuilder.Entity<Place>().HasOne(p => p.Merchant).WithOne(s => s.Place);
        }
    }
}

