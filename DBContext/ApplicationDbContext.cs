using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace IOT.DBContext;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Auditlog> Auditlogs { get; set; }

    public virtual DbSet<ClientMaster> ClientMasters { get; set; }

    public virtual DbSet<SoilReading> SoilReadings { get; set; }

    public virtual DbSet<Usersmaster> Usersmasters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("Server=localhost;Port=3306;Database=iot;User=root;Password=Dhyana@031022;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auditlog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("auditlogs");

            entity.Property(e => e.IpAddress).HasMaxLength(45);
            entity.Property(e => e.Method).HasMaxLength(10);
            entity.Property(e => e.Path).HasMaxLength(2048);
            entity.Property(e => e.QueryString).HasColumnType("text");
            entity.Property(e => e.RequestedAt).HasMaxLength(6);
        });

        modelBuilder.Entity<ClientMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("client_master");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClientCode)
                .HasMaxLength(45)
                .HasColumnName("client_code");
            entity.Property(e => e.ClientName)
                .HasMaxLength(500)
                .HasColumnName("client_name");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("b'1'")
                .HasColumnType("bit(1)")
                .HasColumnName("is_active");
            entity.Property(e => e.IsDelete)
                .HasDefaultValueSql("b'0'")
                .HasColumnType("bit(1)")
                .HasColumnName("is_delete");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("updated_date");
        });

        modelBuilder.Entity<SoilReading>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("soil_reading");

            entity.HasIndex(e => e.Userid, "fk_user_is_idx");

            entity.HasIndex(e => e.Userid, "idx_userid");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("createddate");
            entity.Property(e => e.Ipaddress)
                .HasColumnType("text")
                .HasColumnName("ipaddress");
            entity.Property(e => e.Isactive)
                .HasDefaultValueSql("b'1'")
                .HasColumnType("bit(1)")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdelete)
                .HasDefaultValueSql("b'0'")
                .HasColumnType("bit(1)")
                .HasColumnName("isdelete");
            entity.Property(e => e.Moisturevalue)
                .HasColumnType("json")
                .HasColumnName("moisturevalue");
            entity.Property(e => e.Updatedby).HasColumnName("updatedby");
            entity.Property(e => e.Updateddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("updateddate");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.SoilReadings)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("fk_user_is");
        });

        modelBuilder.Entity<Usersmaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usersmaster");

            entity.HasIndex(e => e.ClientId, "fk_client_id_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.Isactive)
                .HasDefaultValueSql("b'1'")
                .HasColumnType("bit(1)")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdelete)
                .HasDefaultValueSql("b'0'")
                .HasColumnType("bit(1)")
                .HasColumnName("isdelete");
            entity.Property(e => e.MobileNumber)
                .HasMaxLength(10)
                .HasColumnName("mobile_number");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("updated_date");
            entity.Property(e => e.UserName)
                .HasMaxLength(500)
                .HasColumnName("user_name");

            entity.HasOne(d => d.Client).WithMany(p => p.Usersmasters)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_client_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
