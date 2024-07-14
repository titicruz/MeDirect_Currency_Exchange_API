﻿// <auto-generated />
using System;
using MeDirect_Currency_Exchange_API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MeDirect_Currency_Exchange_API.Migrations
{
    [DbContext(typeof(Currency_Exchange_API_Context))]
    [Migration("20240714102256_CreateDatabase")]
    partial class CreateDatabase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.7");

            modelBuilder.Entity("MeDirect_Currency_Exchange_API.Data.Tables.Client", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DT_Create")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("ID")
                        .HasName("IDX_Client_ID");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("MeDirect_Currency_Exchange_API.Data.Tables.Trade", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<DateTime>("Dt_Create")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<decimal>("ExchangeRate")
                        .HasColumnType("decimal(18, 6)");

                    b.Property<string>("FromCurrency")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("TEXT");

                    b.Property<int>("ID_Client")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ToCurrency")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("TEXT");

                    b.HasKey("ID")
                        .HasName("IDX_Trade_ID");

                    b.HasIndex("ID_Client")
                        .HasDatabaseName("IDX_Trade_ID_Client");

                    b.ToTable("Trade");
                });

            modelBuilder.Entity("MeDirect_Currency_Exchange_API.Data.Tables.Trade", b =>
                {
                    b.HasOne("MeDirect_Currency_Exchange_API.Data.Tables.Client", "Client")
                        .WithMany("Trades")
                        .HasForeignKey("ID_Client")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Client_Trades_ID_Client");

                    b.Navigation("Client");
                });

            modelBuilder.Entity("MeDirect_Currency_Exchange_API.Data.Tables.Client", b =>
                {
                    b.Navigation("Trades");
                });
#pragma warning restore 612, 618
        }
    }
}
