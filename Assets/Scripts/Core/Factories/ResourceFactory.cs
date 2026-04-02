using Project.Configs.Catalogs;
using Project.Core.Contracts;
using Project.Core.Domain.Resources;
using Project.Core.Runtime;
using Project.Gameplay.Resources.Views;
using System;
using UnityEditor;
using UnityEngine;

namespace Project.Core.Factories
{
    public class ResourceFactory : IResourceFactory
    {
        private readonly ResourceConfigsCatalog _catalog;
        private readonly ResourceViewPool _viewPool;
        private int _counter = 0;

        public ResourceFactory(ResourceConfigsCatalog catalog, ResourceViewPool viewPool)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _viewPool = viewPool ?? throw new ArgumentNullException(nameof(viewPool));
        }

        public ResourceRuntime Spawn(ResourceType type, Vector3 position)
        {
            var config = _catalog.GetResourceConfig(type);
            var model = new ResourceModel(_counter, config);

            ResourceView view = _viewPool.Pop(type, position);
            var runtime = new ResourceRuntime(model, view);

            runtime.SetPosition(position);
            runtime.Show();

            _counter++;
            return runtime;
        }
    }
}