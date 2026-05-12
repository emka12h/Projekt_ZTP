using GuestService.Models;
using Microsoft.EntityFrameworkCore;

namespace GuestService.Data
{
    public class GuestDbContext : DbContext
    {
        public GuestDbContext(DbContextOptions<GuestDbContext> options)
            : base(options)
        {
        }

        public DbSet<Guest> Guests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Guest>(entity =>
            {
                // Primary Key
                entity.HasKey(g => g.Id);

                // PostgreSQL UUID
                entity.Property(g => g.Id)
                    .ValueGeneratedNever();

                // Required fields
                entity.Property(g => g.WeddingId)
                    .IsRequired();

                entity.Property(g => g.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(g => g.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(g => g.Email)
                    .HasMaxLength(255);

                entity.Property(g => g.PhoneNumber)
                    .HasMaxLength(30);

                // Enums stored as string (czytelniejsze w bazie)
                entity.Property(g => g.RsvpStatus)
                    .HasConversion<string>()
                    .IsRequired();

                entity.Property(g => g.Group)
                    .HasConversion<string>()
                    .IsRequired();

                entity.Property(g => g.Side)
                    .HasConversion<string>()
                    .IsRequired();

                entity.Property(g => g.MealPreference)
                    .HasConversion<string>();

                // Optional fields
                entity.Property(g => g.PlusOneName)
                    .HasMaxLength(200);

                entity.Property(g => g.SeatNumber)
                    .HasMaxLength(20);

                entity.Property(g => g.Allergies)
                    .HasMaxLength(500);

                entity.Property(g => g.Notes)
                    .HasMaxLength(1000);

                // Audit
                entity.Property(g => g.CreatedAt)
                    .IsRequired();

                entity.Property(g => g.UpdatedAt)
                    .IsRequired();

                // Indexes
                entity.HasIndex(g => g.WeddingId);

                entity.HasIndex(g => g.Email);

                entity.HasIndex(g => new { g.WeddingId, g.RsvpStatus });

                entity.HasIndex(g => new { g.WeddingId, g.TableNumber });
            });
        }
    }
}