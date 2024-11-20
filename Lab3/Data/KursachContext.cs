using System;
using System.Collections.Generic;
using Lab3.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab3.Data;

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

    public virtual DbSet<ParkingSpace> ParkingSpaces { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Tariff> Tariffs { get; set; }

    public virtual DbSet<WorkShift> WorkShifts { get; set; }

    public virtual DbSet<WorkShiftsPayment> WorkShiftsPayments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LCR\\SQLEXPRESS;Initial Catalog=Kursach;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");

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

        modelBuilder.Entity<ParkingSpace>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ParkingS__3214EC07A8951081");

            entity.HasOne(d => d.Car).WithMany(p => p.ParkingSpaces)
                .HasForeignKey(d => d.CarId)
                .HasConstraintName("FK_ParkingSpaces_Cars");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payments__3214EC07AF490976");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.TimeIn).HasColumnType("datetime");
            entity.Property(e => e.TimeOut).HasColumnType("datetime");

            entity.HasOne(d => d.Discount).WithMany(p => p.Payments)
                .HasForeignKey(d => d.DiscountId)
                .HasConstraintName("FK__Payments__Discou__73BA3083");

            entity.HasOne(d => d.ParkingSpace).WithMany(p => p.Payments)
                .HasForeignKey(d => d.ParkingSpaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payments_ParkingSpaces");

            entity.HasOne(d => d.Tariff).WithMany(p => p.Payments)
                .HasForeignKey(d => d.TariffId)
                .HasConstraintName("FK__Payments__Tariff__72C60C4A");
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

        modelBuilder.Entity<WorkShiftsPayment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ParkingR__3214EC073998A83B");

            entity.HasOne(d => d.Payment).WithMany(p => p.WorkShiftsPayments)
                .HasForeignKey(d => d.PaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkShiftsPayments_Payments");

            entity.HasOne(d => d.WorkShift).WithMany(p => p.WorkShiftsPayments)
                .HasForeignKey(d => d.WorkShiftId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ParkingRe__WorkS__02084FDA");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
