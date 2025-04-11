using System.Linq.Expressions;
using DataAccess.Abstraction.Entity;
using DataAccess.Abstraction.Repositories;

namespace DataAccess.Repositories
{
    public interface IUserRepository
    {
        Task<bool> ExistsAsync(Expression<Func<UserEntity, bool>> predicate);
    }
}
