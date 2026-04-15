using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HalakAPI.Models;

public partial class HalakContext : DbContext
{
    public HalakContext()
    {
    }

    public HalakContext(DbContextOptions<HalakContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Fogasok> Fogasoks { get; set; }

    public virtual DbSet<Halak> Halaks { get; set; }

    public virtual DbSet<Horgaszok> Horgaszoks { get; set; }

    public virtual DbSet<Tavak> Tavaks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("SERVER=localhost;PORT=3306;DATABASE=halak;USER=root;PASSWORD=;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Fogasok>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("fogasok");

            entity.HasIndex(e => e.HalId, "hal_id");

            entity.HasIndex(e => e.HorgaszId, "horgasz_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Datum)
                .HasColumnType("date")
                .HasColumnName("datum");
            entity.Property(e => e.HalId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11)")
                .HasColumnName("hal_id");
            entity.Property(e => e.HorgaszId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11)")
                .HasColumnName("horgasz_id");

            entity.HasOne(d => d.Hal).WithMany(p => p.Fogasoks)
                .HasForeignKey(d => d.HalId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fogasok_ibfk_1");

            entity.HasOne(d => d.Horgasz).WithMany(p => p.Fogasoks)
                .HasForeignKey(d => d.HorgaszId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fogasok_ibfk_2");
        });

        modelBuilder.Entity<Halak>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("halak");

            entity.HasIndex(e => e.ToId, "to_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Faj)
                .HasMaxLength(100)
                .HasColumnName("faj");
            entity.Property(e => e.Kep)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("blob")
                .HasColumnName("kep");
            entity.Property(e => e.MeretCm)
                .HasPrecision(5)
                .HasColumnName("meret_cm");
            entity.Property(e => e.Nev)
                .HasMaxLength(100)
                .HasColumnName("nev");
            entity.Property(e => e.ToId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11)")
                .HasColumnName("to_id");

            entity.HasOne(d => d.To).WithMany(p => p.Halaks)
                .HasForeignKey(d => d.ToId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("halak_ibfk_1");
        });

        modelBuilder.Entity<Horgaszok>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("horgaszok");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Eletkor)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11)")
                .HasColumnName("eletkor");
            entity.Property(e => e.Nev)
                .HasMaxLength(100)
                .HasColumnName("nev");
        });

        modelBuilder.Entity<Tavak>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tavak");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Helyszin)
                .HasMaxLength(100)
                .HasColumnName("helyszin");
            entity.Property(e => e.Nev)
                .HasMaxLength(100)
                .HasColumnName("nev");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
