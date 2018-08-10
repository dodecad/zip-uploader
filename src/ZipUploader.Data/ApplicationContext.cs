using Microsoft.EntityFrameworkCore;
using ZipUploader.Data.Abstractions;
using ZipUploader.Data.Entities;
using ZipUploader.Data.Extensions;

namespace ZipUploader.Data
{
    /// <summary>
    /// Represents a database context.
    /// </summary>
    public class ApplicationContext : DbContext, IDbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationContext"/> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public ApplicationContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<ArchiveContent> ArchiveItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.RemovePluralizingTableNameConvention();
        }
    }
}
