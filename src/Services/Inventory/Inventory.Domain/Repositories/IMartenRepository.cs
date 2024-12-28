using System.Linq.Expressions;
using BuildingBlocks.Pagination;

namespace Inventory.Domain.Repositories;

public interface IMartenRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default!);

    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default!);
    Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default!);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default!);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default!);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default!);

    Task<PagedResult<TEntity>> GetPageAsync(PaginationRequest paginationRequest ,CancellationToken cancellationToken = default!);

    Task<PagedResult<TEntity>> FindAndPagingAsync(Expression<Func<TEntity, bool>> expression,
        PaginationRequest paginationRequest, CancellationToken cancellationToken = default!);

}