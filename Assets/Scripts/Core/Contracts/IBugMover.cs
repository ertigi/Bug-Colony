using UnityEngine;

namespace Project.Core.Contracts
{
    public interface IBugMover
    {
        void MoveTowards(IBugView view, Vector3 targetPosition, float speed, float deltaTime);
        bool HasReached(IBugView view, Vector3 targetPosition, float distanceThreshold);
    }
}