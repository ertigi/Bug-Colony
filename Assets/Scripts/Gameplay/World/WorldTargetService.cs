using Project.Core.Contracts;
using Project.Core.Domain.Colony;
using Project.Core.Runtime;
using System;
using System.Collections.Generic;

namespace Project.Gameplay.World
{
    public sealed class WorldTargetService
    {
        private readonly ColonyRegistry _colonyState;
        private readonly WorldResourceRegistry _resourceRegistry;

        public WorldTargetService(ColonyRegistry colonyRegistry, WorldResourceRegistry resourceRegistry)
        {
            _colonyState = colonyRegistry ?? throw new ArgumentNullException(nameof(colonyRegistry));
            _resourceRegistry = resourceRegistry ?? throw new ArgumentNullException(nameof(resourceRegistry));
        }

        public List<ITargetable> GetConsumableTargetsForPredator(BugRuntime self)
        {
            var result = new List<ITargetable>();

            foreach (var bug in _colonyState.AliveBugs)
            {
                if (bug == null || bug == self || !bug.IsAvailable)
                {
                    continue;
                }

                result.Add(bug);
            }

            result.AddRange(GetConsumableTargetsForWorker());
            
            return result;
        }

        public List<ITargetable> GetConsumableTargetsForWorker()
        {
            var result = new List<ITargetable>();

            foreach (var resource in _resourceRegistry.ActiveResources)
            {
                if (resource == null || !resource.IsAvailable)
                {
                    continue;
                }

                result.Add(resource);
            }

            return result;
        }

    }
}