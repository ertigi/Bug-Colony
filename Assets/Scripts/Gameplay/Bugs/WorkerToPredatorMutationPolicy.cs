using Project.Configs.Colony;
using Project.Core.Contracts;
using Project.Core.Domain.Bugs;
using Project.Core.Domain.Colony;
using Project.Core.Runtime;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Gameplay.Bugs
{
    public class WorkerToPredatorMutationPolicy : IMutationStrategy
    {
        private readonly ColonyRuleConfig _config;

        public WorkerToPredatorMutationPolicy(ColonyRuleConfig config)
        {
            _config = config;
        }

        public void MutateOffspring(List<OffspringDescriptor> offspring, BugRuntime parent, ColonyRegistry colonyRegistry)
        {
            if (offspring == null || offspring.Count == 0)
                return;

            if (colonyRegistry.AliveBugs.Count <= _config.MutationAliveThreshold)
                return;

            if (Random.value > _config.WorkerToPredatorMutationChance)
                return;

            var index = Random.Range(0, offspring.Count);
            var mutated = new OffspringDescriptor(BugType.Predator, offspring[index].SpawnPosition);
            offspring[index] = mutated;
        }
    }
}