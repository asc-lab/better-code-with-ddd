using System.Linq;
using System.Threading.Tasks;
using LoanApplication.Infrastructure.Common;

namespace LoanApplication.Infrastructure.DataAccess
{
    public interface IGenericRepository<TEntity> where TEntity : Entity
    {
        IQueryable<TEntity> Query();
 
        Task<TEntity> GetById(int id);
 
        Task Create(TEntity entity);
 
        void Update(int id, TEntity entity);
 
        Task Delete(int id);
    }
}