using DataAccess.Abstraction;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    /// <summary>
    /// EF Core implementation of <see cref="IUnitOfWork"/> interface.
    /// </summary>
    /// <typeparam name="TContext">DbContext.</typeparam>
    public class EfCoreUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        /// <summary>
        /// Context of type <see cref="DbContext"/> used to save changes operated by repositories.
        /// </summary>
        private readonly TContext _context;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="context">DbContext.</param>
        public EfCoreUnitOfWork(TContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public void Clear()
        {
            _context.ChangeTracker.Clear();
        }

        /// <inheritdoc/>
        public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task BulkCompleteAsync(bool holdLock, CancellationToken cancellationToken = default)
        {
            BulkConfig bulkConfig = new()
            {
                WithHoldlock = holdLock
            };

            // Using this explicit transaction is mandatory becayse otherwise, if an exception occured during the BulkSaveChanges, no other
            // query can be done with the same dbContext.
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _context.BulkSaveChangesAsync(bulkConfig, cancellationToken: cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
