using UnityEngine;

namespace Project.Core.Contracts
{
    public interface ITargetable
    {
        Vector3 Position { get; }
        bool IsAvailable { get; }
    }
}