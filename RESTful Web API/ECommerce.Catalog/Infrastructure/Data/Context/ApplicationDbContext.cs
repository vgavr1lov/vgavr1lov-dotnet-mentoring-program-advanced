using ECommerce.Catalog.Application.Common.Interfaces;
using ECommerce.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Catalog.Infrastructure.Data.Context;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Category> Category { get; set; }

    public DbSet<Product> Product { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Category:

        modelBuilder.Entity<Category>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<Category>()
            .Property(c => c.Id)
            .IsRequired()
            .HasColumnType("bigint")
            .ValueGeneratedNever();

        modelBuilder.Entity<Category>()
            .Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<Category>()
            .OwnsOne(c => c.Image, image =>
            {
                image.Property(i => i.Url)
                     .HasColumnName("ImageUrl")
                     .HasMaxLength(2048);

                image.Property(i => i.AltText)
                     .HasColumnName("ImageAltText")
                     .HasMaxLength(256);
            });

        modelBuilder.Entity<Category>()
            .Property(c => c.ParentCategoryId)
            .HasColumnType("bigint");

        modelBuilder.Entity<Category>()
            .HasOne<Category>()
            .WithMany()
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Product:

        modelBuilder.Entity<Product>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<Product>()
            .Property(p => p.Id)
            .IsRequired()
            .HasColumnType("bigint")
            .ValueGeneratedNever();

        modelBuilder.Entity<Product>()
            .Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<Product>()
            .Property(p => p.Description)
            .HasMaxLength(256);

        modelBuilder.Entity<Product>()
            .OwnsOne(p => p.Image, image =>
            {
                image.Property(i => i.Url)
                     .HasColumnName("ImageUrl")
                     .HasMaxLength(2048);

                image.Property(i => i.AltText)
                     .HasColumnName("ImageAltText")
                     .HasMaxLength(256);
            });

        modelBuilder.Entity<Product>()
            .Property(p => p.CategoryId)
            .IsRequired()
            .HasColumnType("bigint");

        modelBuilder.Entity<Product>()
            .HasOne<Category>()
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .IsRequired();

        modelBuilder.Entity<Product>()
            .OwnsOne(p => p.Price, price =>
            {
                price.Property(p => p.Amount)
                     .IsRequired()
                     .HasColumnName("PriceAmount")
                     .HasColumnType("NUMERIC(15, 2)");

                price.Property(p => p.Currency)
                     .IsRequired()
                     .HasColumnName("Currency")
                     .HasMaxLength(3);
            });

        modelBuilder.Entity<Product>()
            .Property(p => p.Amount)
            .IsRequired()
            .HasColumnName("Quantity");
    }
}
