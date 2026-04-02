using Cysharp.Threading.Tasks;
using Project.Configs.Colony;
using Project.Core.Contracts;
using Project.Core.Domain.Bugs;
using Project.Core.Domain.Colony;
using Project.Core.Domain.Common;
using Project.Core.Runtime;
using Project.Gameplay.Bugs;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Core.Services
{
    public class BugService
    {
        private readonly BugsRegistry _bugsRegistry;
        private readonly IBugFactory _bugFactory;
        private readonly BugViewPool _bugViewPool;
        private readonly DeathStatisticsService _deathStatisticsService;
        private readonly ISpawnPointProvider _spawnPointProvider;
        private readonly ColonyRuleConfig _colonyRuleConfig;
        private readonly ResourceService _resourceService;
        private bool _isSpawning;

        public BugService(
            BugsRegistry bugsRegistry,
            IBugFactory bugFactory,
            BugViewPool bugViewPool,
            DeathStatisticsService deathStatisticsService,
            ISpawnPointProvider spawnPointProvider,
            ColonyRuleConfig colonyRuleConfig,
            ResourceService resourceService)
        {
            _bugsRegistry = bugsRegistry ?? throw new ArgumentNullException(nameof(bugsRegistry));
            _bugFactory = bugFactory ?? throw new ArgumentNullException(nameof(bugFactory));
            _bugViewPool = bugViewPool ?? throw new ArgumentNullException(nameof(bugViewPool));
            _deathStatisticsService = deathStatisticsService ?? throw new ArgumentNullException(nameof(deathStatisticsService));
            _spawnPointProvider = spawnPointProvider ?? throw new ArgumentNullException(nameof(spawnPointProvider));
            _colonyRuleConfig = colonyRuleConfig ?? throw new ArgumentNullException(nameof(colonyRuleConfig));
            _resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
        }

        public void Activate(BugRuntime bug)
        {
            if (bug == null)
                return;

            _bugsRegistry.Register(bug);
            RunLoop(bug).Forget();

            var policy = bug.LifetimePolicy;
            if (policy == null || !policy.HasLifetime)
                return;

            RunLifetime(bug, policy.GetLifetimeSeconds()).Forget();
        }

        private async UniTaskVoid RunLoop(BugRuntime bug)
        {
            try
            {
                while (!bug.LifetimeToken.IsCancellationRequested && bug.Model.IsAlive)
                {
                    if (bug.CurrentTarget == null || !bug.CurrentTarget.IsAvailable)
                        bug.SetTarget(bug.TargetingStrategy.SelectTarget(bug));

                    if (bug.CurrentTarget == null)
                    {
                        await UniTask.Yield(PlayerLoopTiming.Update, bug.LifetimeToken);
                        continue;
                    }

                    bug.MoveTowardsCurrentTarget(Time.deltaTime);

                    if (bug.HasReachedCurrentTarget())
                        TryConsume(bug, bug.CurrentTarget);

                    await UniTask.Yield(PlayerLoopTiming.Update, bug.LifetimeToken);
                }
            }
            catch (OperationCanceledException)
            {

            }
        }

        public void TryConsume(BugRuntime self, ITargetable target)
        {
            if (self == null || target == null || !self.Model.IsAlive || !target.IsAvailable)
                return;

            if (!self.FeedingPolicy.CanConsume(self, target))
                return;

            if (target is ResourceRuntime resource)
            {
                if (!resource.TryConsume())
                    return;

                _resourceService.Release(resource);
            }
            else if (target is BugRuntime bug)
            {
                if (bug == self || !bug.Model.IsAlive)
                    return;

                Kill(bug, BugDeathReason.Consumed);
            }
            else
            {
                return;
            }

            var nutrition = self.FeedingPolicy.GetNutritionValue(target);
            self.Model.AddConsumed(nutrition);

            self.ClearTarget();
            TryReproduce(self);
        }

        private async UniTaskVoid RunLifetime(BugRuntime bug, float lifetimeSeconds)
        {
            try
            {
                await UniTask.Delay(
                    TimeSpan.FromSeconds(lifetimeSeconds),
                    cancellationToken: bug.LifetimeToken);

                if (bug.Model.IsAlive)
                {
                    Kill(bug, BugDeathReason.LifetimeExpired);
                }
            }
            catch (OperationCanceledException)
            {

            }
        }

        public void Kill(BugRuntime bug, BugDeathReason reason)
        {
            if (bug == null)
                return;

            if (!bug.TryMarkDead())
                return;

            bug.Dispose();
            _bugsRegistry.Unregister(bug);

            if (reason == BugDeathReason.Consumed || reason == BugDeathReason.LifetimeExpired)
                _deathStatisticsService.RegisterDeath(bug.Model.Type);

            _bugViewPool.Push(bug.Model.Type, (BugView)bug.View);

            if (_bugsRegistry.AliveCount == 0)
                TryRespawn();
        }


        private void TryRespawn()
        {
            if (!_colonyRuleConfig.RespawnWorkerIfColonyEmpty || _isSpawning)
                return;

            _isSpawning = true;

            var type = BugType.Worker;
            var position = _spawnPointProvider.GetRandomBugSpawnPoint();

            var bug = _bugFactory.Spawn(type, position);
            Activate(bug);

            _isSpawning = false;
        }

        public void Split(BugRuntime parent, List<OffspringDescriptor> offspring)
        {
            if (parent == null || offspring == null || offspring.Count == 0)
                return;

            Kill(parent, BugDeathReason.Split);

            foreach (var child in offspring)
            {
                var runtime = _bugFactory.Spawn(child.Type, child.SpawnPosition);
                Activate(runtime);
            }
        }

        public void TryReproduce(BugRuntime parent)
        {
            if (parent == null || !parent.Model.IsAlive)
                return;

            var policy = parent.ReproductionPolicy;
            if (policy == null || !policy.ShouldReproduce(parent.Model))
                return;

            var count = policy.GetOffspringCount();
            if (count <= 0)
                return;

            var offspring = new List<OffspringDescriptor>(count);

            for (var i = 0; i < count; i++)
            {
                var pos = _spawnPointProvider.GetSplitSpawnPointNear(parent.Position);
                offspring.Add(new OffspringDescriptor(policy.GetBaseOffspringType(), pos));
            }

            parent.MutationPolicy?.MutateOffspring(offspring, parent, _bugsRegistry);

            Split(parent, offspring);
        }
    }
}