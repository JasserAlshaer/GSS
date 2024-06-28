using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GSS.Models;

public partial class GssContext : DbContext
{
    public GssContext()
    {
    }

    public GssContext(DbContextOptions<GssContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Procudure> Procudures { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<ReportProcudure> ReportProcudures { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=JASSER\\SQLEXPRESS;Database=GSS;Integrated Security=False;Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("Department");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.ToTable("Invoice");

            entity.Property(e => e.IssuedDate).HasColumnType("datetime");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Report).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.ReportId)
                .HasConstraintName("FK_Invoice_Report");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.ToTable("PaymentMethod");

            entity.Property(e => e.Email)
                .HasMaxLength(5000)
                .IsUnicode(false);
            entity.Property(e => e.Expire).HasColumnType("date");
            entity.Property(e => e.Password)
                .HasMaxLength(5000)
                .IsUnicode(false);
            entity.Property(e => e.VisaNumber)
                .HasMaxLength(5000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Procudure>(entity =>
        {
            entity.ToTable("Procudure");

            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Department).WithMany(p => p.Procudures)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_Procudure_Department");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.ToTable("Report");

            entity.Property(e => e.GeneratedDate).HasColumnType("datetime");
            entity.Property(e => e.RequetDate).HasColumnType("datetime");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Reports)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Report_User");
        });

        modelBuilder.Entity<ReportProcudure>(entity =>
        {
            entity.ToTable("ReportProcudure");

            entity.HasOne(d => d.Procudure).WithMany(p => p.ReportProcudures)
                .HasForeignKey(d => d.ProcudureId)
                .HasConstraintName("FK_ReportProcudure_Procudure");

            entity.HasOne(d => d.Report).WithMany(p => p.ReportProcudures)
                .HasForeignKey(d => d.ReportId)
                .HasConstraintName("FK_ReportProcudure_Report");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.BirthDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ImagePath)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Uid)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("UId");

            entity.HasOne(d => d.Department).WithMany(p => p.Users)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_User_Department");

            entity.HasOne(d => d.UserType).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserTypeId)
                .HasConstraintName("FK_User_UserType");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.ToTable("UserType");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
