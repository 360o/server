﻿using _360o.Server.API.V1.Organizations.Model;
using Microsoft.EntityFrameworkCore;

namespace _360o.Server.API.V1.Stores.Model
{
    public class ApiContext : DbContext
    {
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<OfferItem> OfferItems { get; set; }
        public DbSet<Item> Items { get; set; }

        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("postgis");

            modelBuilder.Entity<Organization>().Property(e => e.Name).IsRequired();
            modelBuilder
                .Entity<Organization>()
                .HasGeneratedTsVectorColumn(e => e.EnglishSearchVector, "english", e => new { e.Name, e.EnglishShortDescription, e.EnglishLongDescription, e.EnglishCategoriesJoined })
                .HasIndex(s => s.EnglishSearchVector)
                .HasMethod("GIN");
            modelBuilder
                .Entity<Organization>()
                .HasGeneratedTsVectorColumn(e => e.FrenchSearchVector, "french", e => new { e.Name, e.FrenchShortDescription, e.FrenchLongDescription, e.FrenchCategoriesJoined })
                .HasIndex(s => s.FrenchSearchVector)
                .HasMethod("GIN");

            modelBuilder.Entity<Store>().HasOne(e => e.Organization).WithMany(e => e.Stores);

            modelBuilder.Entity<Place>().Ignore(e => e.Location);
            modelBuilder.Entity<Place>().Property(e => e.Point).HasColumnType("geography (point)");
            modelBuilder.Entity<Place>().HasOne(e => e.Store).WithOne(e => e.Place);

            modelBuilder.Entity<Offer>().Property(e => e.Name).IsRequired();
            modelBuilder.Entity<Offer>().Property(e => e.Discount).HasColumnType("jsonb");
            modelBuilder.Entity<Offer>().HasOne(e => e.Store).WithMany(e => e.Offers);

            modelBuilder.Entity<OfferItem>().HasOne(e => e.Offer).WithMany(e => e.OfferItems);
            modelBuilder.Entity<OfferItem>().HasOne(e => e.Item).WithMany(e => e.OfferItems);

            modelBuilder.Entity<Item>().Property(e => e.Name).IsRequired();
            modelBuilder.Entity<Item>().Property(e => e.Price).HasColumnType("jsonb");
            modelBuilder.Entity<Item>().HasOne(e => e.Store).WithMany(e => e.Items);
        }
    }
}
