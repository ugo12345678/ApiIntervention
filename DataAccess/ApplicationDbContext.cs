using Microsoft.EntityFrameworkCore;
using DataAccess.Abstraction.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DataAccess.Abstraction.Entity;

namespace DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<UserEntity>
    {

        public virtual DbSet<InterventionEntity> Interventions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
