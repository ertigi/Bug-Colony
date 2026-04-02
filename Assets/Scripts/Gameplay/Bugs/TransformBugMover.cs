using Project.Core.Contracts;
using UnityEngine;

namespace Project.Gameplay.Bugs
{
    public class TransformBugMover : IBugMover
    {
        public void MoveTowards(IBugView view, Vector3 targetPosition, float speed, float deltaTime)
        {
            if (view == null || speed <= 0f || deltaTime <= 0f)
            {
                return;
            }

            var currentPosition = view.Position;
            currentPosition = Vector3.MoveTowards(currentPosition, targetPosition, speed * deltaTime);
            view.SetPosition(currentPosition);
        }

        public bool HasReached(IBugView view, Vector3 targetPosition, float distanceThreshold)
        {
            if (view == null)
            {
                return false;
            }

            var sqrDistance = (view.Position - targetPosition).sqrMagnitude;
            return sqrDistance <= distanceThreshold * distanceThreshold;
        }
    }
}