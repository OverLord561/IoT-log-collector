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
    [Migration("20181129180104_Extended + Renamed StandartizedDevice")]
    partial class ExtendedRenamedStandartizedDevice
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("DataProviderCommon.DeviceLogs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateStamp");

                    b.Property<string>("DeviceGuid");

                    b.Property<byte[]>("Message");

                    b.HasKey("Id");

                    b.ToTable("DeviceLogs");
                });
#pragma warning restore 612, 618
        }
    }
}
