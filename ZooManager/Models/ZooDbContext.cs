using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ZooManager.Models;

public partial class ZooDbContext : DbContext
{
    public ZooDbContext()
    {
    }

    public ZooDbContext(DbContextOptions<ZooDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Animal> Animals { get; set; }

    public virtual DbSet<Zoo> Zoos { get; set; }

    public virtual DbSet<ZooAnimal> ZooAnimals { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=AlienPC;Initial Catalog=ZooDB;Persist Security Info=True;User ID=sa;Password=SQLDaMicrosoft;Pooling=False;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Animal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Animal__3214EC078F8D65A2");

            entity.ToTable("Animal");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Zoo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Zoo__3214EC07798001F9");

            entity.ToTable("Zoo");

            entity.Property(e => e.Location)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ZooAnimal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ZooAnima__3214EC07BEA484B3");

            entity.ToTable("ZooAnimal");

            entity.HasOne(d => d.Animal).WithMany(p => p.ZooAnimals)
                .HasForeignKey(d => d.AnimalId)
                .HasConstraintName("AnimalFK");

            entity.HasOne(d => d.Zoo).WithMany(p => p.ZooAnimals)
                .HasForeignKey(d => d.ZooId)
                .HasConstraintName("ZooFK");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
