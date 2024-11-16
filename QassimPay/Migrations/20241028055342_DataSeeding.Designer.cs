﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using QassimPay.Data;

#nullable disable

namespace QassimPay.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241028055342_DataSeeding")]
    partial class DataSeeding
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("QassimPay.Models.AddressModel", b =>
                {
                    b.Property<int>("ADD_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ADD_ID"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Postal_Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Street_Adress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("U_ID")
                        .HasColumnType("integer");

                    b.HasKey("ADD_ID");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("QassimPay.Models.BillingModel", b =>
                {
                    b.Property<int>("Billing_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Billing_ID"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<string>("Billing_number")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("W_ID")
                        .HasColumnType("integer");

                    b.HasKey("Billing_ID");

                    b.ToTable("Billing");
                });

            modelBuilder.Entity("QassimPay.Models.TransferModel", b =>
                {
                    b.Property<int>("Receipt_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Receipt_ID"));

                    b.Property<decimal>("AmountM")
                        .HasColumnType("numeric");

                    b.Property<int>("Reciver")
                        .HasColumnType("integer");

                    b.Property<int>("Sender_ID")
                        .HasColumnType("integer");

                    b.Property<DateTime>("T_date")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Receipt_ID");

                    b.ToTable("Transfer");
                });

            modelBuilder.Entity("QassimPay.Models.UserModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("First_name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Last_name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Monthly_income")
                        .HasColumnType("numeric");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("USER", (string)null);

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Email = "john.doe@example.com",
                            First_name = "John",
                            Last_name = "Doe",
                            Monthly_income = 5000.00m,
                            Password = "password123",
                            Username = "johndoe"
                        },
                        new
                        {
                            ID = 2,
                            Email = "jane.smith@example.com",
                            First_name = "Jane",
                            Last_name = "Smith",
                            Monthly_income = 6000.00m,
                            Password = "password456",
                            Username = "janesmith"
                        });
                });

            modelBuilder.Entity("QassimPay.Models.WalletModel", b =>
                {
                    b.Property<int>("Wallet_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Wallet_ID"));

                    b.Property<decimal>("Balance")
                        .HasColumnType("numeric");

                    b.Property<int>("User_ID")
                        .HasColumnType("integer");

                    b.HasKey("Wallet_ID");

                    b.ToTable("Wallet");
                });
#pragma warning restore 612, 618
        }
    }
}