using HW02.BussinessContext.DB.Entities;

namespace HW02.Helpers
{
    public static class IdGenerationHelper
    {
        public static int GenerateUniqueId<T>(IList<T> entities) where T : ILoggable 
            => entities.Count == 0 ? 1 : entities.Last().Id + 1;
    }
}
