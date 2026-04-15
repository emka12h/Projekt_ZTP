using GuestService.Models;
using Microsoft.EntityFrameworkCore;

namespace GuestService.Data
{
    public class GuestDbContext :DbContext
    {
        public GuestDbContext(DbContextOptions<GuestDbContext> options)  : base(options) {}
        public DbSet<Guest> Guests { get; set; }
    }
}
