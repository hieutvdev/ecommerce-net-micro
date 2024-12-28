using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Abstractions;

public interface IUnitOfWord : IDisposable
{
    Task<int> CommitAsync();
}

public interface IUnitOfWork<TContext> : IUnitOfWord where TContext : DbContext
{}