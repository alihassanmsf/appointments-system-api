using AppointmentSystemApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystemApi.Data
{
    public class AppointmentSystemContext : DbContext
    {
        public AppointmentSystemContext(DbContextOptions<AppointmentSystemContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentStatus> AppointmentStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Custom configurations can go here
        }
    }
}
