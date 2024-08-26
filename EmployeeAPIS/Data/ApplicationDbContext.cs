using EmployeesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPIS.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
      
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employee>()
                .HasOne(c => c.Company)
                .WithMany(e => e.Employees)
                .HasForeignKey(e => e.CompanyId)
                .HasConstraintName("FK_Employees_Companies_CompanyId");
        }
    }
}