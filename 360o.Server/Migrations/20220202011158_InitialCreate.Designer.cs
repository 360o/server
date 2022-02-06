﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;
using _360o.Server.Api.V1.Stores.Model;

#nullable disable

namespace _360o.Server.Migrations
{
    [DbContext(typeof(ApiContext))]
    [Migration("20220202011158_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "postgis");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("_360o.Server.API.V1.Organizations.Model.Organization", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_at");

                    b.Property<List<string>>("EnglishCategories")
                        .IsRequired()
                        .HasColumnType("text[]")
                        .HasColumnName("english_categories");

                    b.Property<string>("EnglishCategoriesJoined")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("english_categories_joined");

                    b.Property<string>("EnglishLongDescription")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("english_long_description");

                    b.Property<NpgsqlTsVector>("EnglishSearchVector")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("tsvector")
                        .HasColumnName("english_search_vector")
                        .HasAnnotation("Npgsql:TsVectorConfig", "english")
                        .HasAnnotation("Npgsql:TsVectorProperties", new[] { "Name", "EnglishShortDescription", "EnglishLongDescription", "EnglishCategoriesJoined" });

                    b.Property<string>("EnglishShortDescription")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("english_short_description");

                    b.Property<List<string>>("FrenchCategories")
                        .IsRequired()
                        .HasColumnType("text[]")
                        .HasColumnName("french_categories");

                    b.Property<string>("FrenchCategoriesJoined")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("french_categories_joined");

                    b.Property<string>("FrenchLongDescription")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("french_long_description");

                    b.Property<NpgsqlTsVector>("FrenchSearchVector")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("tsvector")
                        .HasColumnName("french_search_vector")
                        .HasAnnotation("Npgsql:TsVectorConfig", "french")
                        .HasAnnotation("Npgsql:TsVectorProperties", new[] { "Name", "FrenchShortDescription", "FrenchLongDescription", "FrenchCategoriesJoined" });

                    b.Property<string>("FrenchShortDescription")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("french_short_description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_organizations");

                    b.HasIndex("EnglishSearchVector")
                        .HasDatabaseName("ix_organizations_english_search_vector");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("EnglishSearchVector"), "GIN");

                    b.HasIndex("FrenchSearchVector")
                        .HasDatabaseName("ix_organizations_french_search_vector");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("FrenchSearchVector"), "GIN");

                    b.ToTable("organizations", (string)null);
                });

            modelBuilder.Entity("_360o.Server.API.V1.Stores.Model.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_at");

                    b.Property<string>("EnglishDescription")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("english_description");

                    b.Property<string>("EnglishName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("english_name");

                    b.Property<NpgsqlTsVector>("EnglishSearchVector")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("tsvector")
                        .HasColumnName("english_search_vector")
                        .HasAnnotation("Npgsql:TsVectorConfig", "english")
                        .HasAnnotation("Npgsql:TsVectorProperties", new[] { "EnglishName", "EnglishDescription" });

                    b.Property<string>("FrenchDescription")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("french_description");

                    b.Property<string>("FrenchName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("french_name");

                    b.Property<NpgsqlTsVector>("FrenchSearchVector")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("tsvector")
                        .HasColumnName("french_search_vector")
                        .HasAnnotation("Npgsql:TsVectorConfig", "french")
                        .HasAnnotation("Npgsql:TsVectorProperties", new[] { "FrenchName", "FrenchDescription" });

                    b.Property<MoneyValue?>("Price")
                        .HasColumnType("jsonb")
                        .HasColumnName("price");

                    b.Property<Guid>("StoreId")
                        .HasColumnType("uuid")
                        .HasColumnName("store_id");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_items");

                    b.HasIndex("EnglishSearchVector")
                        .HasDatabaseName("ix_items_english_search_vector");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("EnglishSearchVector"), "GIN");

                    b.HasIndex("FrenchSearchVector")
                        .HasDatabaseName("ix_items_french_search_vector");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("FrenchSearchVector"), "GIN");

                    b.HasIndex("StoreId")
                        .HasDatabaseName("ix_items_store_id");

                    b.ToTable("items", (string)null);
                });

            modelBuilder.Entity("_360o.Server.API.V1.Stores.Model.Offer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_at");

                    b.Property<MoneyValue>("Discount")
                        .HasColumnType("jsonb")
                        .HasColumnName("discount");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid>("StoreId")
                        .HasColumnType("uuid")
                        .HasColumnName("store_id");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_offers");

                    b.HasIndex("StoreId")
                        .HasDatabaseName("ix_offers_store_id");

                    b.ToTable("offers", (string)null);
                });

            modelBuilder.Entity("_360o.Server.API.V1.Stores.Model.OfferItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_at");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("uuid")
                        .HasColumnName("item_id");

                    b.Property<Guid>("OfferId")
                        .HasColumnType("uuid")
                        .HasColumnName("offer_id");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer")
                        .HasColumnName("quantity");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_offer_items");

                    b.HasIndex("ItemId")
                        .HasDatabaseName("ix_offer_items_item_id");

                    b.HasIndex("OfferId")
                        .HasDatabaseName("ix_offer_items_offer_id");

                    b.ToTable("offer_items", (string)null);
                });

            modelBuilder.Entity("_360o.Server.API.V1.Stores.Model.Place", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_at");

                    b.Property<string>("FormattedAddress")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("formatted_address");

                    b.Property<string>("GooglePlaceId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("google_place_id");

                    b.Property<Point>("Point")
                        .IsRequired()
                        .HasColumnType("geography (point)")
                        .HasColumnName("point");

                    b.Property<Guid>("StoreId")
                        .HasColumnType("uuid")
                        .HasColumnName("store_id");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_places");

                    b.HasIndex("StoreId")
                        .IsUnique()
                        .HasDatabaseName("ix_places_store_id");

                    b.ToTable("places", (string)null);
                });

            modelBuilder.Entity("_360o.Server.API.V1.Stores.Model.Store", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_at");

                    b.Property<Guid>("OrganizationId")
                        .HasColumnType("uuid")
                        .HasColumnName("organization_id");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_stores");

                    b.HasIndex("OrganizationId")
                        .HasDatabaseName("ix_stores_organization_id");

                    b.ToTable("stores", (string)null);
                });

            modelBuilder.Entity("_360o.Server.API.V1.Stores.Model.Item", b =>
                {
                    b.HasOne("_360o.Server.API.V1.Stores.Model.Store", "Store")
                        .WithMany("Items")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_items_stores_store_id");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("_360o.Server.API.V1.Stores.Model.Offer", b =>
                {
                    b.HasOne("_360o.Server.API.V1.Stores.Model.Store", "Store")
                        .WithMany("Offers")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_offers_stores_store_id");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("_360o.Server.API.V1.Stores.Model.OfferItem", b =>
                {
                    b.HasOne("_360o.Server.API.V1.Stores.Model.Item", "Item")
                        .WithMany("OfferItems")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_offer_items_items_item_id");

                    b.HasOne("_360o.Server.API.V1.Stores.Model.Offer", "Offer")
                        .WithMany("OfferItems")
                        .HasForeignKey("OfferId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_offer_items_offers_offer_id");

                    b.Navigation("Item");

                    b.Navigation("Offer");
                });

            modelBuilder.Entity("_360o.Server.API.V1.Stores.Model.Place", b =>
                {
                    b.HasOne("_360o.Server.API.V1.Stores.Model.Store", "Store")
                        .WithOne("Place")
                        .HasForeignKey("_360o.Server.API.V1.Stores.Model.Place", "StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_places_stores_store_id");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("_360o.Server.API.V1.Stores.Model.Store", b =>
                {
                    b.HasOne("_360o.Server.API.V1.Organizations.Model.Organization", "Organization")
                        .WithMany("Stores")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_stores_organizations_organization_id");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("_360o.Server.API.V1.Organizations.Model.Organization", b =>
                {
                    b.Navigation("Stores");
                });

            modelBuilder.Entity("_360o.Server.API.V1.Stores.Model.Item", b =>
                {
                    b.Navigation("OfferItems");
                });

            modelBuilder.Entity("_360o.Server.API.V1.Stores.Model.Offer", b =>
                {
                    b.Navigation("OfferItems");
                });

            modelBuilder.Entity("_360o.Server.API.V1.Stores.Model.Store", b =>
                {
                    b.Navigation("Items");

                    b.Navigation("Offers");

                    b.Navigation("Place")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
