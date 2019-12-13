using System.Threading.Tasks;
using LoanApplication.Infrastructure.DataAccess;

namespace LoanApplication.DataAccess
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly LoanApplicationDbContext dbContext;

        public EfUnitOfWork(LoanApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CommitChanges()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}