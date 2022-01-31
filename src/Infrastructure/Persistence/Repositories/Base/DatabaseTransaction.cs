using Application.Interfaces.Repositories.Base;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories.Base
{
    public class DatabaseTransaction : IDatabaseTransaction
    {
        private IDbContextTransaction _transaction;

        public DatabaseTransaction(ApplicationDbContext context)
        {
            _transaction = context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public async Task CommitAsync()
        {
            await _transaction.CommitAsync();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
        }
    }
}
