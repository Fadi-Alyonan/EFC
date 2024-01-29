using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace Shared.Contexts;

public partial class ProductDataContext : DbContext
{
    public ProductDataContext()
    {
    }

    public ProductDataContext(DbContextOptions<ProductDataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Price> Prices { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductionInformation> ProductionInformations { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2B77275EC2");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(200);
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.ManufacturerId).HasName("PK__Manufact__357E5CA10538EAA2");

            entity.Property(e => e.ManufacturerId).HasColumnName("ManufacturerID");
            entity.Property(e => e.ManufacturerName).HasMaxLength(150);
        });

        modelBuilder.Entity<Price>(entity =>
        {
            entity.HasKey(e => e.PriceId).HasName("PK__Prices__4957584FF56E7304");

            entity.Property(e => e.PriceId).HasColumnName("PriceID");
            entity.Property(e => e.PriceDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ProductPrice).HasColumnType("money");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6EDBAFCBEFC");

            entity.Property(e => e.ProductId)
                .ValueGeneratedNever()
                .HasColumnName("ProductID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.ManufacturerId).HasColumnName("ManufacturerID");
            entity.Property(e => e.PriceId).HasColumnName("PriceID");
            entity.Property(e => e.ProductName).HasMaxLength(200);
            entity.Property(e => e.ProductionId).HasColumnName("ProductionID");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Products__Catego__70DDC3D8");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Products)
                .HasForeignKey(d => d.ManufacturerId)
                .HasConstraintName("FK__Products__Manufa__71D1E811");

            entity.HasOne(d => d.Price).WithMany(p => p.Products)
                .HasForeignKey(d => d.PriceId)
                .HasConstraintName("FK__Products__PriceI__73BA3083");

            entity.HasOne(d => d.Production).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductionId)
                .HasConstraintName("FK__Products__Produc__72C60C4A");
        });

        modelBuilder.Entity<ProductionInformation>(entity =>
        {
            entity.HasKey(e => e.ProductionId).HasName("PK__Producti__D5D9A2F582765DDE");

            entity.ToTable("ProductionInformation");

            entity.Property(e => e.ProductionId).HasColumnName("ProductionID");
            entity.Property(e => e.ProductionDate).HasDefaultValueSql("(getdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
