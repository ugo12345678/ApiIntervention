using Business.Abstraction.Exceptions;
using DataAccess.Abstraction.Entities;
using DataAccess.Abstraction.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class InterventionRepository : GenericRepository<InterventionEntity>, IInterventionRepository
    {
        /// <inheritdoc/>
        public InterventionRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<InterventionEntity> GetInterventionWithClientAndTechniciansAsync(long interventionId)
        {
            var clientEntity = await _dbSet
               .Include(v => v.Technician)
               .Include(v => v.Client)
               .Where(d => d.Id == interventionId)
               .FirstOrDefaultAsync();

            if (clientEntity is null)
                throw new ResourceNotFoundException<InterventionEntity>(interventionId);

            return clientEntity;
        }
    }

}
