using DataAccess.Abstraction.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Mapping
{
    public class InterventionMapping : IEntityTypeConfiguration<InterventionEntity>
    {
        public void Configure(EntityTypeBuilder<InterventionEntity> builder)
        {
            builder.ToTable("Intervention");
            builder.HasKey(x => x.Id);
        }
    }
}
