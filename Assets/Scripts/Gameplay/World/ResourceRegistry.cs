using Project.Core.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Gameplay.World
{
    public class ResourceRegistry
    {
        private readonly Dictionary<int, ResourceRuntime> _resources = new();

        public IReadOnlyCollection<ResourceRuntime> ActiveResources => _resources.Values;

        public bool Register(ResourceRuntime resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            return _resources.TryAdd(resource.Model.Id, resource);
        }

        public bool Unregister(ResourceRuntime resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            return _resources.Remove(resource.Model.Id);
        }

        public bool Contains(ResourceRuntime resource)
        {
            if (resource == null)
            {
                return false;
            }

            return _resources.ContainsKey(resource.Model.Id);
        }

        public int Count()
        {
            return _resources.Count;
        }

        public void Clear()
        {
            _resources.Clear();
        }
    }
}