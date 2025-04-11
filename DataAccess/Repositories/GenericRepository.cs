using DataAccess.Abstraction.Entities;
using DataAccess.Abstraction.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntity
    {
        /// <summary>
        /// DbSet managed by the repository
        /// </summary>
        protected readonly DbSet<TEntity> _dbSet;

        /// <summary>
        /// DbContext
        /// </summary>
        protected readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext">EF Core <see cref="DbContext"/></param>
        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? where = null,
                                                         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (where != null)
                query = query.Where(where);

            if (orderBy != null)
                query = orderBy(query);

            return await query.ToListAsync();
        }

        /// <inheritdoc/>
        public virtual async Task<TEntity?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <inheritdoc/>
        public async Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(where);
        }


        /// <inheritdoc/>
        public async Task<TEntity> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (Expression<Func<TEntity, object>> include in includes)
            {
                query = query.Include(include);
            }

            return await query.SingleOrDefaultAsync(where);
        }

        /// <inheritdoc/>
        public async Task AddAsync(params TEntity[] entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        /// <inheritdoc/>
        public void Update(params TEntity[] entities)
        {
            _dbSet.UpdateRange(entities);
        }

        /// <inheritdoc/>
        public void Delete(params TEntity[] entities)
        {
            _dbSet.RemoveRange(entities);
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            bool ret = await _dbSet.AnyAsync(predicate);
            return ret;
        }

        /// <inheritdoc/>
        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }
    }
}
