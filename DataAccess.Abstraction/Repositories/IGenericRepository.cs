using DataAccess.Abstraction.Entities;
using System.Linq.Expressions;

namespace DataAccess.Abstraction.Repositories
{
    /// <summary>
    ///     Generic repository interface.
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity managed by the generic repository</typeparam>
    public interface IGenericRepository<TEntity> where TEntity : class, IEntity
    {
        /// <summary>
        /// Inserts one entity or a range of entities.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        Task AddAsync(params TEntity[] entities);

        /// <summary>
        /// Updates one entity or a range of entities.
        /// </summary>
        /// <param name="entities">The entities to update</param>
        void Update(params TEntity[] entities);

        /// <summary>
        /// Deletes one entity or a range of entities.
        /// </summary>
        /// <param name="entities">The entities to delete</param>
        void Delete(params TEntity[] entities);

        /// <summary>
        /// Finds an entity with its given primary key value.
        /// </summary>
        /// <param name="id">The value of the primary key.</param>
        /// <returns>The found entity or null.</returns>
        Task<TEntity> GetByIdAsync(object id);

        /// <summary>
        /// Gets the first element that satisfies the specified condition in <paramref name="where"/> parameter
        /// or a default value if no such element is found.
        /// </summary>
        /// <param name="where">Condition that the returned entity must satisfy</param>
        /// <param name="includes">Specify the related entities to include in the result</param>
        /// <returns>
        /// The first element of type <typeparamref name="TEntity"/> that satisfies the condition 
        /// or 
        /// Null if no element is found
        /// </returns>
        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Gets the single element that satisfies the specified condition in <paramref name="where"/> parameter
        /// or a default value if no such element is found.
        /// </summary>
        /// <param name="where">Condition that the returned entity must satisfy</param>
        /// <param name="includes">Specify the related entities to include in the result</param>
        /// <returns>
        /// The single element of type <typeparamref name="TEntity"/> that satisfies the condition 
        /// or 
        /// Null if no element is found
        /// </returns>
        /// <remarks>
        ///     If more than one element satisfies the specified condition an exception is thrown
        /// </remarks>
        Task<TEntity> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Gets all the entities of type <typeparamref name="TEntity"/>.
        /// The result can be filtered according the optional parameter <paramref name="where"/>.
        /// The result can be sorted according the optional parameter <paramref name="orderBy"/>
        /// </summary>
        /// <param name="where">Condition that the returned entities must satisfy (optional)</param>
        /// <param name="orderBy">Order clause used to sort the result according a property and a sort order (optional)</param>
        /// <returns>Collection of <see cref="TEntity"/> elements that respect the satisfies parameters</returns>
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> where = null,
                                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        
        /// <summary>
        /// Return a value that indicate if there's one or more item that match predicate
        /// </summary>
        /// <param name="predicate">Condition that the returned entities must satisfy</param>
        /// <returns>true if some entities match the predicate, otherwise false</returns>
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Return the number of entities matching the predicate <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">Condition that counted entities must satisfy.</param>
        /// <returns>The number of entities matching the predicate.</returns>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
