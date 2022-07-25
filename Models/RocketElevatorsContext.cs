using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace RocketElevators.Models
{
    public class RocketElevatorsContext : DbContext
    {
        public RocketElevatorsContext(DbContextOptions<RocketElevatorsContext> options)
            : base(options)
        {
        }

        public DbSet<User> users { get; set; } = null!;
        public DbSet<Battery> batteries { get; set; } = null!;
        public DbSet<Column> columns { get; set; } = null!;
        public DbSet<Elevator> elevators { get; set; } = null!;
        public DbSet<Building> buildings { get; set; } = null!;
        public DbSet<Lead> leads { get; set; } = null!;
    }
}