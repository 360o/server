using System;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace _360o.Server.API.V1.Stores.Model
{
    public class StoresContext : DbContext
    {
        public DbSet<Store> Merchants { get; set; }
        public DbSet<Place> Places { get; set; }

        public StoresContext(DbContextOptions<StoresContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("postgis");

            modelBuilder.Entity<Store>().Property(s => s.EnglishCategories).HasConversion(v => v != null ? string.Join(' ', v) : string.Empty, v => v.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToHashSet());
            modelBuilder.Entity<Store>().Property(s => s.FrenchCategories).HasConversion(v => v != null ? string.Join(' ', v) : string.Empty, v => v.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToHashSet());
            modelBuilder
                .Entity<Store>()
                .HasGeneratedTsVectorColumn(s => s.EnglishSearchVector, "english", s => new { s.DisplayName, s.EnglishShortDescription, s.EnglishLongDescription, s.EnglishCategories })
                .HasIndex(s => s.EnglishSearchVector)
                .HasMethod("GIN");
            modelBuilder
                .Entity<Store>()
                .HasGeneratedTsVectorColumn(s => s.FrenchSearchVector, "french", s => new { s.DisplayName, s.FrenchShortDescription, s.FrenchLongDescription, s.FrenchCategories })
                .HasIndex(s => s.FrenchSearchVector)
                .HasMethod("GIN");

            modelBuilder.Entity<Place>().Ignore(p => p.Location);
            modelBuilder.Entity<Place>().Property(p => p.Point).HasColumnType("geography (point)");
            modelBuilder.Entity<Place>().HasOne(p => p.Merchant).WithOne(s => s.Place);
        }
    }
}

