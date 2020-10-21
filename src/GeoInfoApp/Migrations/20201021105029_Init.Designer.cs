﻿// <auto-generated />
using System;
using GeoInfoApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeoInfoApp.Migrations
{
    [DbContext(typeof(GeoAppDbContext))]
    [Migration("20201021105029_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9");

            modelBuilder.Entity("GeoInfoApp.History.GeoInfoHistoryItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<float>("CurrentTemperatureCelsius")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("DateTimeUtc")
                        .HasColumnType("TEXT");

                    b.Property<string>("TimeZone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("GeoInfoHistoryItems");
                });
#pragma warning restore 612, 618
        }
    }
}
