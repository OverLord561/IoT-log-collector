﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MySQLDataProviderPlugin;

namespace MySQLDataProviderPlugin.Migrations
{
    [DbContext(typeof(MySQLDbContext))]
    [Migration("20181126134731_Empty")]
    partial class Empty
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("DataProviderFacade.StandardizedDevice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateStamp");

                    b.Property<byte[]>("Message");

                    b.HasKey("Id");

                    b.ToTable("GeneralDevices");
                });
#pragma warning restore 612, 618
        }
    }
}
