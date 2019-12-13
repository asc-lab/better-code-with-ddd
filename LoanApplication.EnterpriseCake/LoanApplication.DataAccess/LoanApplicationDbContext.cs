using LoanApplication.BusinessLogic;
using Microsoft.EntityFrameworkCore;

namespace LoanApplication.DataAccess
{
    public class LoanApplicationDbContext : DbContext
    {
        public LoanApplicationDbContext(DbContextOptions<LoanApplicationDbContext> options) : base(options)
        {
            
        }
        
        public DbSet<BusinessLogic.LoanApplication> LoanApplications { get; set; }
        
        public DbSet<BusinessLogic.Operator> Operators { get; set; }
        
        
    }
}