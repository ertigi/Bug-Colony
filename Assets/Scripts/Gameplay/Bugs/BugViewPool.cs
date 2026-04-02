using System;
using System.Collections.Generic;
using Project.Configs.Catalogs;
using Project.Core.Domain.Bugs;
using UnityEngine;

namespace Project.Gameplay.Bugs
{
    public class BugViewPool
    {
        private readonly BugConfigsCatalog _catalog;
        private readonly Dictionary<BugType, Stack<BugView>> _pool = new();
        private readonly Transform _poolRoot;

        public BugViewPool(BugConfigsCatalog catalog)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _poolRoot = new GameObject("BugViewPool").transform;
        }

        public BugView Pop(BugType type, Vector3 position)
        {
            if (!_pool.TryGetValue(type, out var stack))
            {
                stack = new Stack<BugView>();
                _pool[type] = stack;
            }

            BugView view;
            if (stack.Count > 0)
            {
                view = stack.Pop();
            }
            else
            {
                var prefab = _catalog.GetBugConfig(type).Prefab;

                if (prefab == null)
                    throw new ArgumentNullException($"empty prefab {type}");
                
                view = UnityEngine.Object.Instantiate(prefab).GetComponent<BugView>();
            }

            view.ResetView();
            view.SetPosition(position);
            view.Show();
            return view;
        }

        public void Push(BugType type, BugView view)
        {
            if (view == null)
            {
                return;
            }

            if (!_pool.TryGetValue(type, out var stack))
            {
                stack = new Stack<BugView>();
                _pool[type] = stack;
            }

            view.ResetView();
            view.Hide();
            view.transform.SetParent(_poolRoot);
            stack.Push(view);
        }
    }
}