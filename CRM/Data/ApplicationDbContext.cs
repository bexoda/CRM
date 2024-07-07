using CRM.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRM.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Visitor> Visitors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Add indexing
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Email)
                .IsUnique();

            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Company)
                .IsUnique();

            modelBuilder.Entity<Appointment>()
                .HasIndex(a => new { a.StaffId, a.AppointmentDate });

            modelBuilder.Entity<Visitor>()
                .HasIndex(v => v.Email)
                .IsUnique();

            modelBuilder.Entity<Visitor>()
                .HasIndex(v => v.ContactNumber)
                .IsUnique();

            // Configure relationship between AppUser and Employee
            modelBuilder.Entity<AppUser>()
                .HasOne(u => u.Employee)
                .WithMany()
                .HasForeignKey(u => u.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ensure EmployeeId allows null values
            modelBuilder.Entity<AppUser>()
                .Property(u => u.EmployeeId)
                .IsRequired(false);
        }
    }
}
