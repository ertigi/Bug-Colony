using UnityEngine;

namespace Project.Core.Contracts
{
    public interface ISpawnPointProvider
    {
        Vector3 GetRandomBugSpawnPoint();
        Vector3 GetRandomResourceSpawnPoint();
        Vector3 GetSplitSpawnPointNear(Vector3 center);
    }
}