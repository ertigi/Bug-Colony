using Project.Configs.Catalogs;
using Project.Core.Contracts;
using Project.Core.Domain.Bugs;
using Project.Core.Runtime;
using Project.Gameplay.Bugs;
using System;
using UnityEngine;

namespace Project.Core.Factories
{
    public class BugFactory : IBugFactory
    {
        private readonly BugConfigsCatalog _catalog;
        private readonly BugViewPool _viewPool;
        private readonly BugBehaviourFactory _profileFactory;
        private int _counter = 0;

        public BugFactory(
            BugConfigsCatalog catalog,
            BugViewPool viewPool,
            BugBehaviourFactory profileFactory)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _viewPool = viewPool ?? throw new ArgumentNullException(nameof(viewPool));
            _profileFactory = profileFactory ?? throw new ArgumentNullException(nameof(profileFactory));
        }

        public BugRuntime Spawn(BugType type, Vector3 position)
        {
            var config = _catalog.GetBugConfig(type);
            var model = new BugModel(_counter, config);
            var profile = _profileFactory.Create(type);

            BugView view = _viewPool.Pop(type, position);

            var runtime = new BugRuntime(
                model,
                view,
                profile.Mover,
                profile.TargetingStrategy,
                profile.FeedingPolicy,
                profile.ReproductionPolicy,
                profile.MutationPolicy,
                profile.LifetimePolicy);

            runtime.SetPosition(position);
            runtime.Show();

            _counter++;
            return runtime;
        }
    }
}