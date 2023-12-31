﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace laundromatweb.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20230805151728_records")]
    partial class records
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.0");

            modelBuilder.Entity("Customer", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CreatedById")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Item", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CreatedById")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Price")
                        .HasColumnType("REAL");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("LaundryRecord", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CreatedById")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CustomerId")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("DiscountPrice")
                        .HasColumnType("REAL");

                    b.Property<bool?>("IsDiscount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<double?>("PriceSnapshot")
                        .HasColumnType("REAL");

                    b.Property<string>("Remark")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Weight")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("CustomerId");

                    b.ToTable("LaundryRecords");
                });

            modelBuilder.Entity("RecordItem", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CreatedById")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Qty")
                        .HasColumnType("REAL");

                    b.Property<int?>("RecordID")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("RecordID");

                    b.ToTable("RecordItems");
                });

            modelBuilder.Entity("Store", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CreatedById")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<double?>("PricePerWeight")
                        .HasColumnType("REAL");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("User", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CreatedById")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Customer", b =>
                {
                    b.HasOne("User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("Item", b =>
                {
                    b.HasOne("User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("LaundryRecord", b =>
                {
                    b.HasOne("User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");

                    b.Navigation("CreatedBy");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("RecordItem", b =>
                {
                    b.HasOne("User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("LaundryRecord", "Record")
                        .WithMany("RecordItems")
                        .HasForeignKey("RecordID");

                    b.Navigation("CreatedBy");

                    b.Navigation("Record");
                });

            modelBuilder.Entity("Store", b =>
                {
                    b.HasOne("User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("User", b =>
                {
                    b.HasOne("User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("LaundryRecord", b =>
                {
                    b.Navigation("RecordItems");
                });
#pragma warning restore 612, 618
        }
    }
}
