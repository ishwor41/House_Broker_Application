using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HouseBroker.Domain.Entities;

namespace HouseBroker.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Your tables
        public DbSet<Property> Properties { get; set; }


        public DbSet<CommissionSetting> CommissionSettings { get; set; }
    }
}