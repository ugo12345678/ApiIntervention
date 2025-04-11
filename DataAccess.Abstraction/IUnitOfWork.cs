namespace DataAccess.Abstraction
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Empty the context containing the tracked entities.
        /// </summary>
        void Clear();

        /// <summary>
        /// Persist the tracked entities.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
        Task<int> CompleteAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Persist the tracked entities using bulk operations.
        /// </summary>
        /// <param name="holdLock">
        /// Whether the merge operation should put a lock on the rows.
        /// This pararameter should be set to false if concurrent calls to this function can happend on the same table. However, in this
        /// situation, the application must garantee that a same line cannot be inserted/updated concurrently.
        /// </param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        Task BulkCompleteAsync(bool holdLock, CancellationToken cancellationToken = default);
    }
}
