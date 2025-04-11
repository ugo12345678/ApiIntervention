
using DataAccess.Abstraction.Entity;
using DataAccess.Abstraction.Enums;

namespace DataAccess.Abstraction.Entities
{
    public class InterventionEntity : ITrackedEntity, IEntity
    {
        public long Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public required ServiceTypeData ServiceType { get; set; }
        public MaterialTypeData? MaterialType { get; set; }
        public required UserEntity Client { get; set; }
        public ICollection<UserEntity>? Technician { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
