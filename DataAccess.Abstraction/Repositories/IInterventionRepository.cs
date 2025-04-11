using DataAccess.Abstraction.Entities;

namespace DataAccess.Abstraction.Repositories
{
    public interface IInterventionRepository : IGenericRepository<InterventionEntity>
    {
        public Task<InterventionEntity> GetInterventionWithClientAndTechniciansAsync(long interventionId);
    }
}
