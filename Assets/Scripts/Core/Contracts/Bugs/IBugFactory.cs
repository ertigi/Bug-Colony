using Project.Core.Domain.Bugs;
using Project.Core.Runtime;
using UnityEngine;

namespace Project.Core.Contracts
{
    public interface IBugFactory
    {
        BugRuntime Spawn(BugType type, Vector3 position);
    }
}