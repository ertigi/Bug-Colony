using Project.Core.Runtime;
using UnityEngine;

namespace Project.Core.Contracts
{
    public interface ITargetingStrategy
    {
        ITargetable SelectTarget(BugRuntime self);
    }
}