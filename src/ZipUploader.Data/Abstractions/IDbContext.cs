using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZipUploader.Data.Entities;

namespace ZipUploader.Data.Abstractions
{
    /// <summary>
    /// Resolving a DbContext as an interface.
    /// </summary>
    public interface IDbContext : IDisposable
    {
        DbSet<ArchiveContent> ArchiveItems { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        int SaveChanges();

        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}
