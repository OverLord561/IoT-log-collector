﻿// <auto-generated />
using System;
using MSSQlDataProviderPlugin;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MSSQLDataProviderPlugin.Migrations
{
    [DbContext(typeof(MSSQLDbContext))]
    partial class MSSQLDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DataProviderCommon.StandardizedDevice", b =>
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
