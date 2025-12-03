using Microsoft.EntityFrameworkCore;
using PhotoBooker.Domain.Entities;
using PhotoBooker.Domain.Enums;

namespace PhotoBooker.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Photographer> Photographers { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Portfolio> Portfolios { get; set; }
    public DbSet<PortfolioImage> PortfolioImages { get; set; }
    public DbSet<Package> Packages { get; set; }
    public DbSet<ShootingCategory> ShootingCategories { get; set; }
    public DbSet<Availability> Availabilities { get; set; }
    public DbSet<BookingRequest> BookingRequests { get; set; }
    public DbSet<Shooting> Shootings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure TPH (Table Per Hierarchy) inheritance for User
        modelBuilder.Entity<User>()
            .HasDiscriminator<UserRole>("Role")
            .HasValue<User>(UserRole.Unspecified)
            .HasValue<Photographer>(UserRole.Photographer)
            .HasValue<Client>(UserRole.Client);

        // Configure unique index on Username
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<Portfolio>()
            .HasOne(p => p.Photographer)
            .WithMany(ph => ph.Portfolios)
            .HasForeignKey(p => p.PhotographerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure PortfolioImage
        modelBuilder.Entity<PortfolioImage>()
            .HasOne(pi => pi.Portfolio)
            .WithMany(p => p.Images)
            .HasForeignKey(pi => pi.PortfolioId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Package>()
            .HasOne(p => p.Photographer)
            .WithMany(ph => ph.Packages)
            .HasForeignKey(p => p.PhotographerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Package>()
            .HasOne(p => p.ShootingCategory)
            .WithMany(sc => sc.Packages)
            .HasForeignKey(p => p.ShootingCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Availability>()
            .HasOne(a => a.Photographer)
            .WithMany(ph => ph.Availabilities)
            .HasForeignKey(a => a.PhotographerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BookingRequest>()
            .HasOne(br => br.Client)
            .WithMany(c => c.SentBookingRequests)
            .HasForeignKey(br => br.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BookingRequest>()
            .HasOne(br => br.Photographer)
            .WithMany(ph => ph.ReceivedBookingRequests)
            .HasForeignKey(br => br.PhotographerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BookingRequest>()
            .HasOne(br => br.Package)
            .WithMany(p => p.BookingRequests)
            .HasForeignKey(br => br.PackageId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Shooting>()
            .HasOne(s => s.Client)
            .WithMany(c => c.ClientShootings)
            .HasForeignKey(s => s.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Shooting>()
            .HasOne(s => s.Photographer)
            .WithMany(ph => ph.PhotographerShootings)
            .HasForeignKey(s => s.PhotographerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Shooting>()
            .HasOne(s => s.Package)
            .WithMany(p => p.Shootings)
            .HasForeignKey(s => s.PackageId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Package>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);
    }
}

