﻿// <auto-generated />
using System;
using AutomatedPassengerBoardingKiosk.API.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AutomatedPassengerBoardingKiosk.API.Migrations
{
    [DbContext(typeof(FlightManifestContext))]
    [Migration("20220609035756_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("AutomatedPassengerBoardingKiosk.API.Entities.BoardingPass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("BoardingPassValidation")
                        .HasColumnType("bit");

                    b.Property<string>("Class")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.Property<bool>("DobValidation")
                        .HasColumnType("bit");

                    b.Property<bool>("FaceValidation")
                        .HasColumnType("bit");

                    b.Property<int>("FlightNumber")
                        .HasColumnType("int");

                    b.Property<bool>("NameValidation")
                        .HasColumnType("bit");

                    b.Property<string>("PassengerDocumentNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Seat")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.HasKey("Id");

                    b.HasIndex("FlightNumber");

                    b.HasIndex("PassengerDocumentNumber");

                    b.ToTable("BoardingPasses");
                });

            modelBuilder.Entity("AutomatedPassengerBoardingKiosk.API.Entities.Flight", b =>
                {
                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<DateTime>("BoardingDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Carrier")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("From")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Gate")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<string>("To")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Number");

                    b.ToTable("Flights");
                });

            modelBuilder.Entity("AutomatedPassengerBoardingKiosk.API.Entities.Person", b =>
                {
                    b.Property<string>("DocumentNumber")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<DateTime>("Dob")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DocumentExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.HasKey("DocumentNumber");

                    b.ToTable("People");
                });

            modelBuilder.Entity("AutomatedPassengerBoardingKiosk.API.Entities.BoardingPass", b =>
                {
                    b.HasOne("AutomatedPassengerBoardingKiosk.API.Entities.Flight", "Flight")
                        .WithMany()
                        .HasForeignKey("FlightNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AutomatedPassengerBoardingKiosk.API.Entities.Person", "Passenger")
                        .WithMany()
                        .HasForeignKey("PassengerDocumentNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Flight");

                    b.Navigation("Passenger");
                });
#pragma warning restore 612, 618
        }
    }
}
