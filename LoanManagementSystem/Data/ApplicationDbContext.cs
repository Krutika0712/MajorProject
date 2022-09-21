using LoanManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
        }
        public DbSet<LoanCategory> LoanCategories { get; set; }
        public DbSet<LoanManagementSystem.Models.Plan> Plan { get; set; }
        public DbSet<LoanManagementSystem.Models.Customer> Customer { get; set; }
        public DbSet<LoanManagementSystem.Models.Approval> Approval { get; set; }

    }
}
