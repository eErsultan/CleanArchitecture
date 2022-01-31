using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Base;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Persistence.Repositories.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IArticleRepository Articles { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Articles = new ArticleRepository(_context);
        }

        public IDatabaseTransaction BeginTransaction()
        {
            return new DatabaseTransaction(_context);
        }
    }
}
