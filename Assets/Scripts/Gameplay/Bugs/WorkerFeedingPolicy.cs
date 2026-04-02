using Project.Core.Contracts;
using Project.Core.Domain.Common;
using Project.Core.Runtime;

namespace Project.Gameplay.Bugs
{
    public class WorkerFeedingPolicy : IFeedingStrategy
    {
        public bool CanConsume(BugRuntime self, ITargetable target)
        {
            if (self == null || target == null || !target.IsAvailable)
                return false;

            return target.TargetType == TargetType.Resource;
        }

        public int GetNutritionValue(ITargetable target)
        {
            if (target is ResourceRuntime resource)
                return resource.Model.NutritionValue;

            return 0;
        }
    }
}