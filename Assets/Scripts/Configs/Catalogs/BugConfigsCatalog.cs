using Project.Configs.Bugs;
using Project.Core.Domain.Bugs;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Configs.Catalogs
{
    [CreateAssetMenu(fileName = "BugConfigsCatalogSO", menuName = "Project/Configs/Bug Config Catalog")]
    public class BugConfigsCatalog : ScriptableObject
    {
        [SerializeField] private List<BugConfig> _configs = new();

        private Dictionary<BugType, BugConfig> _cache;

        public IReadOnlyList<BugConfig> Configs => _configs;

        public BugConfig GetBugConfig(BugType type)
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

            _cache = new Dictionary<BugType, BugConfig>(_configs.Count);

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