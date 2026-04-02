using Project.Core.Runtime;
using UnityEngine;

namespace Project.Core.Contracts
{
    public interface IFeedingStrategy
    {
        bool CanConsume(BugRuntime self, ITargetable target);
        int GetNutritionValue(ITargetable target);
    }
}