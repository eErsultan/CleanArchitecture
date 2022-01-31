using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories.Base;
using Application.Interfaces.Repositories;

namespace Infrastructure.Persistence.Repositories
{
    public class ArticleRepository : GenericRepositoryAsync<Article, int>, IArticleRepository
    {
        private readonly DbSet<Article> _articles;

        public ArticleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _articles = dbContext.Set<Article>();
        }
    }
}
