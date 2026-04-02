using Project.Core.Contracts;
using Project.Core.Domain.Colony;
using Project.Core.Runtime;
using System;
using System.Collections.Generic;

namespace Project.Core.Services
{
    public class BugReproductionService
    {
        private readonly IBugFactory _bugFactory;
        private readonly ISpawnPointProvider _spawnPointProvider;
        private readonly BugsRegistry _bugsRegistry;

        public BugReproductionService(
            IBugFactory bugFactory,
            ISpawnPointProvider spawnPointProvider,
            BugsRegistry bugsRegistry)
        {
            _bugFactory = bugFactory ?? throw new ArgumentNullException(nameof(bugFactory));
            _spawnPointProvider = spawnPointProvider ?? throw new ArgumentNullException(nameof(spawnPointProvider));
            _bugsRegistry = bugsRegistry ?? throw new ArgumentNullException(nameof(bugsRegistry));
        }

        public bool TryCreateOffspring(BugRuntime parent, out List<BugRuntime> offspring)
        {
            offspring = null;

            if (parent == null || !parent.Model.IsAlive)
                return false;

            var policy = parent.ReproductionStrategy;
            if (policy == null || !policy.ShouldReproduce(parent.Model))
                return false;

            var count = policy.GetOffspringCount();
            if (count <= 0)
                return false;

            var descriptors = new List<OffspringDescriptor>(count);
            var positions = _spawnPointProvider.GetSplitSpawnPointNear(parent.Position, count);

            for (var i = 0; i < count; i++)
            {
                descriptors.Add(new OffspringDescriptor(policy.GetBaseOffspringType(), positions[i]));
            }

            parent.MutationStrategy?.MutateOffspring(descriptors, parent, _bugsRegistry);

            offspring = new List<BugRuntime>(descriptors.Count);

            foreach (var descriptor in descriptors)
                offspring.Add(_bugFactory.Spawn(descriptor.Type, descriptor.SpawnPosition));

            return offspring.Count > 0;
        }
    }
}