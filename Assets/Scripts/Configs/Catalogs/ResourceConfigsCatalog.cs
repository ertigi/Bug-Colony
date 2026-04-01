using Project.Configs.Resources;
using Project.Core.Domain.Resources;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Configs.Catalogs
{
    [CreateAssetMenu(fileName = "ResourceConfigsCatalogSO", menuName = "Project/Configs/Resource Configs Catalog")]
    public sealed class ResourceConfigsCatalog : ScriptableObject
    {
        [SerializeField] private List<ResourceConfig> _configs = new();

        private Dictionary<ResourceType, ResourceConfig> _cache;

        public IReadOnlyList<ResourceConfig> Configs => _configs;

        public ResourceConfig Get(ResourceType type)
        {
            BuildCacheIfNeeded();

            if (_cache.TryGetValue(type, out var config))
                return config;

            throw new KeyNotFoundException($"{type} not found in {name}");
        }

        private void BuildCacheIfNeeded()
        {
            if (_cache != null)
                return;

            _cache = new Dictionary<ResourceType, ResourceConfig>(_configs.Count);

            foreach (var config in _configs)
            {
                if (!_cache.TryAdd(config.Type, config))
                {
                    throw new InvalidOperationException(
                        $"Duplicate {config.Type} in catalog {name}");
                }
            }
        }
    }
}