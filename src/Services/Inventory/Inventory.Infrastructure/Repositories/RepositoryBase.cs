
using System.Linq.Expressions;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Pagination;
using Inventory.Domain.Repositories;
using Inventory.Infrastructure.Data;
using JasperFx.Core.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories;

public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
{

    private readonly ApplicationDbContext _context;
    private readonly DbSet<TEntity>? _dbSet;

    public RepositoryBase(ApplicationDbContext dbContext)
    {
        _context = dbContext;
        _dbSet = _context.Set<TEntity>();
    }

    private static readonly Func<DbContext, Task<List<TEntity>>> GetAllCompiledQuery =
        EF.CompileAsyncQuery<DbContext, List<TEntity>>(context => context.Set<TEntity>()!.ToList());

    private static readonly Func<DbContext, Expression<Func<TEntity, bool>>, Task<List<TEntity>>> FindCompiledQuery =
        EF.CompileAsyncQuery<DbContext, Expression<Func<TEntity, bool>>, List<TEntity>>(
            (context, expression) => context.Set<TEntity>().Where(expression).ToList()
        );
    private static readonly Func<DbContext, Func<TEntity, bool>, Task<TEntity>> GetCompiledQuery =
        EF.CompileAsyncQuery<DbContext, Func<TEntity, bool>, TEntity>(
            (context, func) => context.Set<TEntity>().FirstOrDefault(func)!
        );
    private static readonly Func<DbContext, string, object, Task<TEntity>> GetByFieldCompiledQuery =
        EF.CompileAsyncQuery<DbContext, string, object, TEntity>(
            (context, fieldName, value) => context.Set<TEntity>()
                .FirstOrDefault(e => EF.Property<object>(e, fieldName).Equals(value))!
        );
    
    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await GetAllCompiledQuery(_context);
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        return await FindCompiledQuery(_context, expression);
    }

    public async Task<TEntity> GetAsync(Func<TEntity, bool> func, CancellationToken cancellationToken = default)
    {
        var entity = await _dbSet!.FindAsync(func);
        return entity!;
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet!.AddAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync(Func<TEntity, bool> func, object payload, CancellationToken cancellationToken = default)
    {
        var entity = await _dbSet!.FindAsync(func) ?? throw new NotFoundException($"{typeof(TEntity).Name} not found");
        _context.Entry(entity).CurrentValues.SetValues(payload);
    }

    public async Task DeleteAsync(Func<TEntity, bool> func, CancellationToken cancellationToken = default)
    {
        var entity = await _dbSet!.FindAsync(func) ?? throw new NotFoundException($"{typeof(TEntity).Name} not found");
        _dbSet.Remove(entity);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await _dbSet!.AddRangeAsync(entities, cancellationToken);
    }

    public async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        _dbSet!.UpdateRange(entities);
    }

    public async Task<IEnumerable<TResult>> GetSelectAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>>? conditions, CancellationToken cancellationToken = default)
    {
        if (conditions != null)
            return await _dbSet!.Where(conditions).Select(selector).ToListAsync(cancellationToken);
        return await _dbSet!.Select(selector).ToListAsync(cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<TEntity> GetByFieldAsync(string filedName, object value,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet!.AsNoTracking().FirstOrDefaultAsync(e => EF.Property<object>(e, filedName).Equals(value), cancellationToken) ??
               throw new NotFoundException($"{typeof(TEntity).Name} not found");
    }

    public async Task<PaginatedResult<TEntity>> GetPageAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
    {
        int pageIndex = paginationRequest.PageIndex >= 0 ? paginationRequest.PageIndex : 0;
        var pageSize = paginationRequest.PageSize >= 1 ? paginationRequest.PageSize : 1;
        var totalCount = await _dbSet!.AsNoTracking().LongCountAsync(cancellationToken);
        
        IEnumerable<TEntity> entities = await _dbSet!.AsNoTracking().Skip(pageSize*pageIndex).Take(pageSize).ToListAsync(cancellationToken);
        var pagedResult = new PaginatedResult<TEntity>(pageIndex, pageSize, totalCount, entities);
        return pagedResult;
    }
}