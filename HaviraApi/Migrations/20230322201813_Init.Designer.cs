﻿// <auto-generated />
using System;
using HaviraApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HaviraApi.Migrations
{
    [DbContext(typeof(HaviraDbContext))]
    [Migration("20230322201813_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HaviraApi.Entities.Dish", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("CreatedTimestamp")
                        .HasColumnType("bigint");

                    b.Property<string>("Desc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("GroupId")
                        .HasColumnType("bigint");

                    b.Property<long>("LastMadeTimestamp")
                        .HasColumnType("bigint");

                    b.Property<int>("NofRatings")
                        .HasColumnType("int");

                    b.Property<long>("OwnerId")
                        .HasColumnType("bigint");

                    b.Property<float>("Rating")
                        .HasColumnType("real");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("Dishes");
                });

            modelBuilder.Entity("HaviraApi.Entities.Group", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("CreatedTimestamp")
                        .HasColumnType("bigint");

                    b.Property<string>("JoinCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("HaviraApi.Entities.UserProfile", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long?>("GroupId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("UserProfiles");
                });

            modelBuilder.Entity("HaviraApi.Entities.Dish", b =>
                {
                    b.HasOne("HaviraApi.Entities.Group", null)
                        .WithMany("Dishes")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HaviraApi.Entities.UserProfile", b =>
                {
                    b.HasOne("HaviraApi.Entities.Group", null)
                        .WithMany("UserProfiles")
                        .HasForeignKey("GroupId");
                });

            modelBuilder.Entity("HaviraApi.Entities.Group", b =>
                {
                    b.Navigation("Dishes");

                    b.Navigation("UserProfiles");
                });
#pragma warning restore 612, 618
        }
    }
}
