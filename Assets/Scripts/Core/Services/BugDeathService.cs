using Project.Core.Domain.Colony;
using Project.Core.Domain.Common;
using Project.Core.Runtime;
using Project.Gameplay.Bugs;
using System;

namespace Project.Core.Services
{
    public class BugDeathService
    {
        private readonly BugsRegistry _bugsRegistry;
        private readonly BugViewPool _bugViewPool;
        private readonly DeathStatisticsService _deathStatisticsService;

        public BugDeathService(
            BugsRegistry bugsRegistry,
            BugViewPool bugViewPool,
            DeathStatisticsService deathStatisticsService)
        {
            _bugsRegistry = bugsRegistry ?? throw new ArgumentNullException(nameof(bugsRegistry));
            _bugViewPool = bugViewPool ?? throw new ArgumentNullException(nameof(bugViewPool));
            _deathStatisticsService = deathStatisticsService ?? throw new ArgumentNullException(nameof(deathStatisticsService));
        }

        public bool Kill(BugRuntime bug, BugDeathReason reason)
        {
            if (bug == null)
                return false;

            if (!bug.TryMarkDead())
                return false;

            bug.Dispose();
            _bugsRegistry.Unregister(bug);

            if (reason == BugDeathReason.Consumed || reason == BugDeathReason.LifetimeExpired)
                _deathStatisticsService.RegisterDeath(bug.Model.Type);

            _bugViewPool.Push(bug.Model.Type, (BugView)bug.View);
            return true;
        }
    }
}