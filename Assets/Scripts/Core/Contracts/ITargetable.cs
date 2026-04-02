using Project.Core.Domain.Common;
using UnityEngine;

namespace Project.Core.Contracts
{
    public interface ITargetable
    {
        TargetType TargetType { get; }
        Vector3 Position { get; }
        bool IsAvailable { get; }
    }
}