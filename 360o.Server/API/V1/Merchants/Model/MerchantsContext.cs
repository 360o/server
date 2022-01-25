using System;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace _360o.Server.Merchants.API.V1.Model
{
    public class MerchantsContext : DbContext
    {
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<Place> Places { get; set; }

        public MerchantsContext(DbContextOptions<MerchantsContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("postgis");

            modelBuilder.Entity<Merchant>().HasIndex(m => m.UserId).IsUnique();
            modelBuilder.Entity<Merchant>().Property(m => m.EnglishCategories).HasConversion(v => v != null ? string.Join(' ', v) : string.Empty, v => v.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToHashSet());
            modelBuilder.Entity<Merchant>().Property(m => m.FrenchCategories).HasConversion(v => v != null ? string.Join(' ', v) : string.Empty, v => v.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToHashSet());
            modelBuilder
                .Entity<Merchant>()
                .HasGeneratedTsVectorColumn(m => m.EnglishSearchVector, "english", m => new { m.DisplayName, m.EnglishShortDescription, m.EnglishLongDescription, m.EnglishCategories })
                .HasIndex(m => m.EnglishSearchVector)
                .HasMethod("GIN");
            modelBuilder
                .Entity<Merchant>()
                .HasGeneratedTsVectorColumn(m => m.FrenchSearchVector, "french", m => new { m.DisplayName, m.FrenchShortDescription, m.FrenchLongDescription, m.FrenchCategories })
                .HasIndex(m => m.FrenchSearchVector)
                .HasMethod("GIN");

            modelBuilder.Entity<Place>().Ignore(p => p.Location);
            modelBuilder.Entity<Place>().Property(p => p.Point).HasColumnType("geography (point)");
            modelBuilder.Entity<Place>().HasOne(p => p.Merchant).WithMany(m => m.Places);
        }
    }
}

