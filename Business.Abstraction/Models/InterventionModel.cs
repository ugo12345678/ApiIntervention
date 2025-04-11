using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Abstraction.Enums;

namespace Business.Abstraction.Models
{
    public class InterventionModel
    {
        public string? Name { get; set; } = string.Empty;
        public required ServiceTypeData ServiceType { get; set; }
        public MaterialTypeData? MaterialType { get; set; }
        public required string ClientName { get; set; }
        public ICollection<string>? TechniciansNames { get; set; }
    }

    public class GetInterventionModel
    {
        public string? Name { get; set; } = string.Empty;
        public required ServiceTypeData ServiceType { get; set; }
        public MaterialTypeData? MaterialType { get; set; }
        public required UserModel Client { get; set; }
        public ICollection<UserModel>? Technicians { get; set; }
    }
}
