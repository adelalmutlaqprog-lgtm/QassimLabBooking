using Microsoft.EntityFrameworkCore;
using QassimLabBooking.Models;

namespace QassimLabBooking.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Lab> Labs { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
    }
}