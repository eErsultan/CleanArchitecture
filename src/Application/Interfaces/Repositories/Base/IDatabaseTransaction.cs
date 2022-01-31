using System;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories.Base
{
    public interface IDatabaseTransaction
    {
        void Commit();
        Task CommitAsync();
        void Rollback();
        Task RollbackAsync();
    }
}
