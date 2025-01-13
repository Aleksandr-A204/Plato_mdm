﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Plato.MDM.Storage.Data;

#nullable disable

namespace Plato.MDM.DataAccess.Postgres.Migrations
{
    [DbContext(typeof(MdmDbContext))]
    [Migration("20250110085637_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("public")
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "postgis");
            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Plato.MDM.Storage.Models.MdmDirectoryDomainEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id")
                        .HasName("mdm_DirectoryDomain_pkey");

                    b.HasIndex(new[] { "Name" }, "mdm_DirectoryDomain_Name_key")
                        .IsUnique();

                    b.ToTable("mdm_DirectoryDomain", "public");
                });

            modelBuilder.Entity("Plato.MDM.Storage.Models.MdmDirectoryEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<Guid?>("DirectoryDomainId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DirectoryLevelId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id")
                        .HasName("mdm_Directory_pkey");

                    b.HasIndex("DirectoryDomainId");

                    b.HasIndex("DirectoryLevelId");

                    b.ToTable("mdm_Directory", "public");
                });

            modelBuilder.Entity("Plato.MDM.Storage.Models.MdmDirectoryLevelEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id")
                        .HasName("mdm_DirectoryLevel_pkey");

                    b.HasIndex(new[] { "Name" }, "mdm_DirectoryLevel_Name_key")
                        .IsUnique();

                    b.ToTable("mdm_DirectoryLevel", "public");
                });

            modelBuilder.Entity("Plato.MDM.Storage.Models.MdmDirectoryVersionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("DataSourceDate")
                        .HasColumnType("text");

                    b.Property<string>("DataSourceName")
                        .HasColumnType("text");

                    b.Property<string>("DataSourceUrl")
                        .HasColumnType("text");

                    b.Property<Guid?>("DirectoryId")
                        .HasColumnType("uuid");

                    b.Property<string>("TableName")
                        .HasColumnType("text");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateOnly?>("VersionDate")
                        .HasColumnType("date");

                    b.Property<string>("VersionDescription")
                        .HasColumnType("text");

                    b.HasKey("Id")
                        .HasName("mdm_DirectoryVersion_pkey");

                    b.HasIndex("DirectoryId");

                    b.ToTable("mdm_DirectoryVersion", "public");
                });

            modelBuilder.Entity("Plato.MDM.Storage.Models.MdmDirectoryEntity", b =>
                {
                    b.HasOne("Plato.MDM.Storage.Models.MdmDirectoryDomainEntity", "DirectoryDomain")
                        .WithMany("MdmDirectories")
                        .HasForeignKey("DirectoryDomainId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("mdm_Directory_DirectoryDomainId_fkey");

                    b.HasOne("Plato.MDM.Storage.Models.MdmDirectoryLevelEntity", "DirectoryLevel")
                        .WithMany("MdmDirectories")
                        .HasForeignKey("DirectoryLevelId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("mdm_Directory_DirectoryLevelId_fkey");

                    b.Navigation("DirectoryDomain");

                    b.Navigation("DirectoryLevel");
                });

            modelBuilder.Entity("Plato.MDM.Storage.Models.MdmDirectoryVersionEntity", b =>
                {
                    b.HasOne("Plato.MDM.Storage.Models.MdmDirectoryEntity", "Directory")
                        .WithMany("MdmDirectoryVersions")
                        .HasForeignKey("DirectoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("mdm_DirectoryVersion_DirectoryId_fkey");

                    b.Navigation("Directory");
                });

            modelBuilder.Entity("Plato.MDM.Storage.Models.MdmDirectoryDomainEntity", b =>
                {
                    b.Navigation("MdmDirectories");
                });

            modelBuilder.Entity("Plato.MDM.Storage.Models.MdmDirectoryEntity", b =>
                {
                    b.Navigation("MdmDirectoryVersions");
                });

            modelBuilder.Entity("Plato.MDM.Storage.Models.MdmDirectoryLevelEntity", b =>
                {
                    b.Navigation("MdmDirectories");
                });
#pragma warning restore 612, 618
        }
    }
}
