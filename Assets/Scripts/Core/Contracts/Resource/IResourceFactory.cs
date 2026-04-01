using Project.Core.Domain.Resources;
using Project.Core.Runtime;
using UnityEngine;

namespace Project.Core.Contracts
{
    public interface IResourceFactory
    {
        ResourceRuntime Spawn(ResourceType type, Vector3 position);
    }
}