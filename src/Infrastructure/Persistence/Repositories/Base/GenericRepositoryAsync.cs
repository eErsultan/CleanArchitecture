using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.Repositories.Base;

namespace Infrastructure.Persistence.Repositories.Base
{
    public class GenericRepositoryAsync<T, Y> : IGenericRepositoryAsync<T, Y> where T : BaseEntity<Y>
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepositoryAsync(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CountAsync()
        {
            return await _dbContext.Set<T>().CountAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _dbContext
                .Set<T>()
                .Where(x => x.IsDeleted == false)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(Y id)
        {
            return await _dbContext
                .Set<T>()
                .Where(x => x.IsDeleted == false && 
                              x.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task AddRangeAsync(ICollection<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(ICollection<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(ICollection<T> entities)
        {
            _dbContext.Set<T>().UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsExistAsync(Y id)
        {
            return await _dbContext
                .Set<T>()
                .AnyAsync(p => p.IsDeleted == false && 
                                 p.Id.Equals(id));
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
