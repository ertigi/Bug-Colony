using UnityEngine;

namespace Project.Core.Contracts
{
    public interface ITargetingStrategy
    {
        void SelectTarget(Vector3 self);
    }
}