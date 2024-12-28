using Auth.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace Auth.Application.Data;
public interface IApplicationDbContext 
{
    DbSet<Key> Keys { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}