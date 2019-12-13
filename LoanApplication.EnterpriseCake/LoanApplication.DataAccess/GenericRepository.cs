using System.Linq;
using System.Threading.Tasks;
using LoanApplication.Infrastructure.Common;
using LoanApplication.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace LoanApplication.DataAccess
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : Entity
    {
        private readonly LoanApplicationDbContext dbContext;

        public GenericRepository(LoanApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<TEntity> Query()
        {
            return dbContext.Set<TEntity>();
        }

        public async Task<TEntity> GetById(int id)
        {
            return await dbContext.Set<TEntity>()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task Create(TEntity entity)
        {
            await dbContext.Set<TEntity>().AddAsync(entity);
        }

        public void Update(int id, TEntity entity)
        {
            dbContext.Set<TEntity>().Update(entity);
        }

        public async Task Delete(int id)
        {
            var entity = await GetById(id);
            dbContext.Set<TEntity>().Remove(entity);
        }
    }
}