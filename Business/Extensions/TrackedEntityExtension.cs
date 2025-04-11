using DataAccess.Abstraction.Entities;

namespace Business.Extensions
{
    public static class TrackedEntityExtension
    {
        public static void SetCreationData(this ITrackedEntity trackedEntity, string userName)
        {
            DateTime time = DateTime.UtcNow;
            trackedEntity.CreatedBy = userName;
            trackedEntity.CreatedOn = time;

            trackedEntity.SetUpdateData(userName, time);
        }

        public static void SetUpdateData(this ITrackedEntity trackedEntity, string userName, DateTime? updateTime = null)
        {
            trackedEntity.UpdatedBy = userName;
            trackedEntity.UpdatedOn = updateTime ?? DateTime.UtcNow;
        }

    }
}