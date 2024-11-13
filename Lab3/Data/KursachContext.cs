using System;
using System.Collections.Generic;
using Lab2proj.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Lab2proj.Data;

public partial class KursachContext : DbContext
{
    public KursachContext()
    {
    }

    public KursachContext(DbContextOptions<KursachContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeMonthlyShift> EmployeeMonthlyShifts { get; set; }

    public virtual DbSet<ParkingRecord> ParkingRecords { get; set; }

    public virtual DbSet<ParkingRecordsWorkShift> ParkingRecordsWorkShifts { get; set; }

    public virtual DbSet<ParkingSpace> ParkingSpaces { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<RegularClient> RegularClients { get; set; }

    public virtual DbSet<Tariff> Tariffs { get; set; }

    public virtual DbSet<WorkShift> WorkShifts { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    string getStringFrom = "DefaultConnection";
    //    string connectionString = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetConnectionString(getStringFrom);
    //}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cars__3214EC07FF6150EB");

            entity.Property(e => e.Brand)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Number)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Client).WithMany(p => p.Cars)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cars__ClientId__5629CD9C");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Clients__3214EC078921F3CF");

            entity.Property(e => e.MiddleName)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Surname)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Telephone)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Discount__3214EC07E60C16F3");

            entity.Property(e => e.Description)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC07DF787F27");

            entity.Property(e => e.MiddleName)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Surname)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EmployeeMonthlyShift>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("EmployeeMonthlyShifts");

            entity.Property(e => e.FullName)
                .HasMaxLength(92)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ParkingRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ParkingR__3214EC07AF4FFF4E");

            entity.Property(e => e.TimeIn).HasColumnType("datetime");
            entity.Property(e => e.TimeOut).HasColumnType("datetime");

            entity.HasOne(d => d.Car).WithMany(p => p.ParkingRecords)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ParkingRe__CarId__797309D9");

            entity.HasOne(d => d.ParkingSpace).WithMany(p => p.ParkingRecords)
                .HasForeignKey(d => d.ParkingSpaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ParkingRe__Parki__7A672E12");

            entity.HasOne(d => d.Payment).WithMany(p => p.ParkingRecords)
                .HasForeignKey(d => d.PaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ParkingRe__Payme__7B5B524B");
        });

        modelBuilder.Entity<ParkingRecordsWorkShift>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ParkingR__3214EC073998A83B");

            entity.HasOne(d => d.ParkingRecord).WithMany(p => p.ParkingRecordsWorkShifts)
                .HasForeignKey(d => d.ParkingRecordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ParkingRe__Parki__02FC7413");

            entity.HasOne(d => d.WorkShift).WithMany(p => p.ParkingRecordsWorkShifts)
                .HasForeignKey(d => d.WorkShiftId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ParkingRe__WorkS__02084FDA");
        });

        modelBuilder.Entity<ParkingSpace>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ParkingS__3214EC07A8951081");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payments__3214EC07AF490976");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");

            entity.HasOne(d => d.Discount).WithMany(p => p.Payments)
                .HasForeignKey(d => d.DiscountId)
                .HasConstraintName("FK__Payments__Discou__73BA3083");

            entity.HasOne(d => d.Tariff).WithMany(p => p.Payments)
                .HasForeignKey(d => d.TariffId)
                .HasConstraintName("FK__Payments__Tariff__72C60C4A");
        });

        modelBuilder.Entity<RegularClient>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("RegularClients");

            entity.Property(e => e.Brand)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(92)
                .IsUnicode(false);
            entity.Property(e => e.Number)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Telephone)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Tariff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tariffs__3214EC07DB23FD9C");

            entity.Property(e => e.Description)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Rate).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<WorkShift>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkShif__3214EC0721F12D1B");

            entity.Property(e => e.ShiftEndTime).HasColumnType("datetime");
            entity.Property(e => e.ShiftStartTime).HasColumnType("datetime");

            entity.HasOne(d => d.Employee).WithMany(p => p.WorkShifts)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__WorkShift__Emplo__5BE2A6F2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
