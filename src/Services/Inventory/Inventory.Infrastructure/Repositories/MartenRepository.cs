using System.Linq.Expressions;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Pagination;
using Inventory.Domain.Repositories;
using Marten;

namespace Inventory.Infrastructure.Repositories;

public class MartenRepository<TEntity>(IDocumentSession document) : IMartenRepository<TEntity>
    where TEntity : class
{
    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await document.Query<TEntity>().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        return await document.Query<TEntity>().Where(expression).ToListAsync(cancellationToken);
    }

    public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await document.LoadAsync<TEntity>(id, cancellationToken) ?? throw new NotFoundException($"{typeof(TEntity).Name} is notfound");
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        document.Store(entity);
        await document.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        document.Update(entity);
        await document.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        TEntity entityFound = await GetByIdAsync(id, cancellationToken);
        document.Delete(entityFound);
        await document.SaveChangesAsync(cancellationToken);
    }

    public async Task<PagedResult<TEntity>> GetPageAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
    {
        int pageIndex = paginationRequest.PageIndex;
        int pageSize = paginationRequest.PageSize;
        IQueryable<TEntity> queryable = document.Query<TEntity>().AsQueryable();
        PagedResult<TEntity> pagedResult = await PagedResult<TEntity>.CreateAsync(queryable, pageIndex, pageSize);
        return pagedResult;
    }

    public async Task<PagedResult<TEntity>> FindAndPagingAsync(Expression<Func<TEntity, bool>> expression, PaginationRequest paginationRequest,
        CancellationToken cancellationToken = default)
    {
        int pageIndex = paginationRequest.PageIndex;
        int pageSize = paginationRequest.PageSize;
        IQueryable<TEntity> queryable = document.Query<TEntity>().Where(expression).AsQueryable();
        PagedResult<TEntity> pagedResult = await PagedResult<TEntity>.CreateAsync(queryable, pageIndex, pageSize);
        return pagedResult;
    }
}