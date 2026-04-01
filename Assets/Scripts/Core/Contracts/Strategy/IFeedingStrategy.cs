using UnityEngine;

namespace Project.Core.Contracts
{
    public interface IFeedingStrategy
    {
        bool CanConsume(Vector3 self, Vector3 target);
        int GetNutritionValue(Vector3 target);
    }
}