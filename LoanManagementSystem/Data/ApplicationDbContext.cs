using LoanManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Data
{
    public class ApplicationDbContext
        :IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
        }
        public DbSet<LoanCategory> LoanCategories { get; set; }
        public DbSet<Plan> Plan { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Approval> Approval { get; set; }

    }
}
