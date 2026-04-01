using Project.Core.Domain.Bugs;
using UnityEngine;

namespace Project.Core.Contracts
{
    public interface IBugFactory
    {
        void Spawn(BugType type, Vector3 position);
    }
}