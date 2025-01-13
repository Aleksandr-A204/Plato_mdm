//using Humanizer.Configuration;
using Microsoft.EntityFrameworkCore;
using Plato.MDM.Storage.Models;

namespace Plato.MDM.Storage.Data;

public partial class MdmDbContext : DbContext
{
    public MdmDbContext(DbContextOptions<MdmDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MdmDirectoryEntity> MdmDirectories { get; set; }

    public virtual DbSet<MdmDirectoryDomainEntity> MdmDirectoryDomains { get; set; }

    public virtual DbSet<MdmDirectoryLevelEntity> MdmDirectoryLevels { get; set; }

    public virtual DbSet<MdmDirectoryVersionEntity> MdmDirectoryVersions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //if (optionsBuilder.IsConfigured)
        //{
        //IConfigurationRoot config = new ConfigurationBuilder()
        //    .AddJsonFile("appsettings.json")
        //    .SetBasePath(Directory.GetCurrentDirectory())
        //    .Build();
        //optionsBuilder.UseNpgsql(config);
        //}
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("postgis");
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.HasDefaultSchema("mdm");

        modelBuilder.Entity<MdmDirectoryEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("mdm_Directory_pkey");

            entity.ToTable("mdm_Directory");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.DirectoryDomain).WithMany(p => p.MdmDirectories)
                .HasForeignKey(d => d.DirectoryDomainId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("mdm_Directory_DirectoryDomainId_fkey");

            entity.HasOne(d => d.DirectoryLevel).WithMany(p => p.MdmDirectories)
                .HasForeignKey(d => d.DirectoryLevelId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("mdm_Directory_DirectoryLevelId_fkey");
        });

        modelBuilder.Entity<MdmDirectoryDomainEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("mdm_DirectoryDomain_pkey");

            entity.ToTable("mdm_DirectoryDomain");

            entity.HasIndex(e => e.Name, "mdm_DirectoryDomain_Name_key").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<MdmDirectoryLevelEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("mdm_DirectoryLevel_pkey");

            entity.ToTable("mdm_DirectoryLevel");

            entity.HasIndex(e => e.Name, "mdm_DirectoryLevel_Name_key").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<MdmDirectoryVersionEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("mdm_DirectoryVersion_pkey");

            entity.ToTable("mdm_DirectoryVersion");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Version).HasMaxLength(50);
            entity.Property(e => e.VersionDate);

            entity.HasOne(d => d.Directory).WithMany(p => p.MdmDirectoryVersions)
                .HasForeignKey(d => d.DirectoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("mdm_DirectoryVersion_DirectoryId_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
