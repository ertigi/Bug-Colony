using Project.Configs.Catalogs;
using Project.Core.Domain.Resources;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Gameplay.Resources.Views
{
    public class ResourceViewPool
    {
        private readonly ResourceConfigsCatalog _catalog;
        private readonly Dictionary<ResourceType, Stack<ResourceView>> _pool = new();
        private readonly Transform _poolRoot;

        public ResourceViewPool(ResourceConfigsCatalog catalog)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _poolRoot = new GameObject("[ResourceViewPool]").transform;
        }

        public ResourceView Pop(ResourceType type, Vector3 position)
        {
            if (!_pool.TryGetValue(type, out var stack))
            {
                stack = new Stack<ResourceView>();
                _pool[type] = stack;
            }

            ResourceView view;
            if (stack.Count > 0)
            {
                view = stack.Pop();
            }
            else
            {
                var prefab = _catalog.GetResourceConfig(type).Prefab;

                if (prefab == null)
                    throw new ArgumentNullException($"empty prefab {type}");
                
                view = UnityEngine.Object.Instantiate(prefab).GetComponent<ResourceView>();
            }

            view.ResetView();
            view.SetPosition(position);
            view.Show();
            return view;
        }

        public void Push(ResourceType type, ResourceView view)
        {
            if (view == null)
            {
                return;
            }

            if (!_pool.TryGetValue(type, out var stack))
            {
                stack = new Stack<ResourceView>();
                _pool[type] = stack;
            }

            view.ResetView();
            view.Hide();
            view.transform.SetParent(_poolRoot);
            stack.Push(view);
        }

        public void Clear()
        {
            _pool.Clear();
        }
    }
}