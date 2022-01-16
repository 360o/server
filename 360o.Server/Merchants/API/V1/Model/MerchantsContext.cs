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
            modelBuilder.Entity<Merchant>().HasIndex(m => new { m.DisplayName }).IsTsVectorExpressionIndex("simple");

            modelBuilder.Entity<Place>().Ignore(m => m.Location);
            modelBuilder.Entity<Place>().Property(p => p.Point).HasColumnType("geography (point)");
            modelBuilder.Entity<Place>().HasOne(p => p.Merchant).WithMany(m => m.Places);
        }
    }
}

