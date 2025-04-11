using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DataAccess.Abstraction.Entity;
using DataAccess.Repositories;
using DataAccess;

public class UserRepository : IUserRepository
{

    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<bool> ExistsAsync(Expression<Func<UserEntity, bool>> predicate)
    {
        return await _context.Set<UserEntity>().AnyAsync(predicate);
    }
}
