using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TimeBasedPreventiveMeasures.Models;
using TimeBasedPreventiveMeasures.Models.Data;

namespace TimeBasedPreventiveMeasures.Data
{
    public class LifecycleManagementDB : DbContext
    {
        public LifecycleManagementDB(DbContextOptions<LifecycleManagementDB> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.AssigendItCustodian)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.AssignedITCustodianId);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Custodian> Custodians { get; set; }
        public DbSet<ActionPlan> ActionPlans { get; set; }

        public DbSet<Notification> Notifications { get; set; }
    }
}
