using Domain.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories.Base
{
    public interface IGenericRepositoryAsync<T, Y> where T : BaseEntity<Y>
    {
        Task<T> GetByIdAsync(Y id);
        Task<IReadOnlyList<T>> GetAllAsync(int pageNumber, int pageSize);
        Task<int> CountAsync();
        Task<T> AddAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task AddRangeAsync(ICollection<T> entities);
        Task DeleteRangeAsync(ICollection<T> entities);
        Task UpdateRangeAsync(ICollection<T> entities);
        Task<bool> IsExistAsync(Y id);
        Task SaveChangesAsync();
    }
}