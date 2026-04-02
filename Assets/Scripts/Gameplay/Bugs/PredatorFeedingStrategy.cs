using Project.Core.Contracts;
using Project.Core.Domain.Common;
using Project.Core.Runtime;

namespace Project.Gameplay.Bugs
{
    public class PredatorFeedingStrategy : IFeedingStrategy
    {
        public bool CanConsume(BugRuntime self, ITargetable target)
        {
            if (self == null || target == null || !target.IsAvailable)
                return false;

            if (target.TargetType == TargetType.Resource)
                return true;

            if (target.TargetType == TargetType.Bug)
            {
                var bug = (BugRuntime)target;
                return bug != self && bug.Model.IsAlive;
            }

            return false;
        }

        public int GetNutritionValue(ITargetable target)
        {
            if (target is ResourceRuntime resource)
                return resource.Model.NutritionValue;

            if (target is BugRuntime bug)
                return bug.Model.NutritionValue;

            return 0;
        }
    }
}