using System.Threading.Tasks;

namespace LoanApplication.Infrastructure.DataAccess
{
    public interface IUnitOfWork
    {
        Task CommitChanges();
    }
}