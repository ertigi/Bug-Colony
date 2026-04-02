using Project.Configs.Colony;
using Project.Core.Contracts;
using System;
using UnityEngine;
using static UnityEngine.Rendering.STP;
using Random = UnityEngine.Random;

namespace Project.Gameplay.World
{
    public class SceneBoundsSpawnPointProvider : ISpawnPointProvider
    {
        private readonly SpawnAreaConfig _config;

        public SceneBoundsSpawnPointProvider(SpawnAreaConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public Vector3 GetRandomBugSpawnPoint()
        {
            return GetPointInsideBounds();
        }

        public Vector3 GetRandomResourceSpawnPoint()
        {
            return GetPointInsideBounds();
        }

        public Vector3[] GetSplitSpawnPointNear(Vector3 center, int count)
        {
            var points = new Vector3[count];

            for (int i = 0; i < count; i++)
            {
                var offset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
                offset = offset.normalized * _config.SplitOffset;

                var point = center + offset;
                points[i] = ClampToBounds(point);
            }

            return points;
        }

        private Vector3 GetPointInsideBounds()
        {
            var point = InsideBounds(_config.GetBounds());
            return ClampToBounds(point);
        }

        private Vector3 ClampToBounds(Vector3 point)
        {
            var bounds = _config.GetBounds();

            point.x = Mathf.Clamp(point.x, bounds.min.x, bounds.max.x);
            point.y = bounds.center.y;
            point.z = Mathf.Clamp(point.z, bounds.min.z, bounds.max.z);

            return point;
        }

        private Vector3 InsideBounds(Bounds bounds)
        {
            return new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z));
        }
    }
}