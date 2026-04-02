using Project.Core.Domain.Bugs;
using UnityEngine;

namespace Project.Core.Runtime
{
    public struct OffspringDescriptor
    {
        public BugType Type { get; }
        public Vector3 SpawnPosition { get; }

        public OffspringDescriptor(BugType type, Vector3 spawnPosition)
        {
            Type = type;
            SpawnPosition = spawnPosition;
        }
    }
}