﻿// <auto-generated />
using System;
using Autocorrect.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Autocorrect.API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Autocorrect.API.Data.DbEntities.Licenses", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ExpiresOn");

                    b.Property<byte[]>("LicenseFile");

                    b.Property<int>("MaxUtilization");

                    b.Property<int>("Status");

                    b.Property<Guid>("UserId");

                    b.Property<int>("Utilized");

                    b.HasKey("Id");

                    b.ToTable("Licenses");
                });

            modelBuilder.Entity("Autocorrect.API.Models.SpecialWord", b =>
                {
                    b.Property<string>("WrongWord")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateAdded");

                    b.Property<DateTime?>("DateRetreived");

                    b.Property<DateTime?>("DateUpdated");

                    b.Property<string>("RightWord");

                    b.HasKey("WrongWord");

                    b.ToTable("SpecialWords");
                });
#pragma warning restore 612, 618
        }
    }
}
