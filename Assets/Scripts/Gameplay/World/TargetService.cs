using Project.Core.Contracts;
using Project.Core.Domain.Bugs;
using Project.Core.Domain.Colony;
using Project.Core.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Gameplay.World
{
    public class TargetService
    {
        private readonly BugsRegistry _bugsRegistry;
        private readonly ResourceRegistry _resourceRegistry;

        public TargetService(BugsRegistry bugsRegistry, ResourceRegistry resourceRegistry)
        {
            _bugsRegistry = bugsRegistry ?? throw new ArgumentNullException(nameof(bugsRegistry));
            _resourceRegistry = resourceRegistry ?? throw new ArgumentNullException(nameof(resourceRegistry));
        }

        public ITargetable GetNearestTargets(BugRuntime self, BugType bugType)
        {
            var targets = bugType == BugType.Worker ? ConsumableTargetsForWorker() : ConsumableTargetsForPredator(self);
            return NearestTarget(targets, self);
        }

        public ITargetable GetRandomTargets(BugRuntime self, BugType bugType)
        {
            var targets = bugType == BugType.Worker ? ConsumableTargetsForWorker() : ConsumableTargetsForPredator(self);
            return RandomTarget(targets);
        }

        private ITargetable NearestTarget(List<ITargetable> targets, BugRuntime self)
        {
            ITargetable bestTarget = null;
            var bestDistanceSqr = float.MaxValue;

            foreach (var target in targets)
            {
                if (target == null || !target.IsAvailable)
                    continue;

                var distanceSqr = (target.Position - self.Position).sqrMagnitude;
                if (distanceSqr >= bestDistanceSqr)
                    continue;

                bestTarget = target;
                bestDistanceSqr = distanceSqr;
            }

            return bestTarget;
        }

        private ITargetable RandomTarget(List<ITargetable> targets)
        {
            if(targets == null || targets.Count == 0)
                return null;

            var rand = UnityEngine.Random.Range(0, targets.Count);
            return targets[rand];
        }

        private List<ITargetable> ConsumableTargetsForWorker()
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

        private List<ITargetable> ConsumableTargetsForPredator(BugRuntime self)
        {
            var result = new List<ITargetable>();

            foreach (var bug in _bugsRegistry.AliveBugs)
            {
                if (bug == null || bug == self || !bug.IsAvailable)
                {
                    continue;
                }

                result.Add(bug);
            }

            result.AddRange(ConsumableTargetsForWorker());
            
            return result;
        }
    }
}