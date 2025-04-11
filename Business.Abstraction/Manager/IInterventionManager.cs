using Business.Abstraction.Models;

namespace Business.Abstraction.Manager
{
    public interface IInterventionManager
    {
        Task<long> CreateAsync(InterventionModel intervention,string username);
        Task<long> UpdateAsync(long interventionId, InterventionModel intervention,string username);
        Task DeleteAsync(long interventionId);
        Task<GetInterventionModel> GetByIdAsync(long interventionId);
        Task<List<InterventionModel>> SearchAsync(bool isAdmin,string username);
    }
}
