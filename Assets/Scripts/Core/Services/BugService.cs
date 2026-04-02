using Project.Configs.Colony;
using Project.Core.Contracts;
using Project.Core.Domain.Bugs;
using Project.Core.Domain.Colony;
using Project.Core.Domain.Common;
using Project.Core.Runtime;
using Project.Gameplay.Bugs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Project.Core.Services
{
    public class BugService : IDisposable
    {
        private readonly BugsRegistry _bugsRegistry;
        private readonly IBugFactory _bugFactory;
        private readonly ISpawnPointProvider _spawnPointProvider;
        private readonly ColonyRuleConfig _colonyRuleConfig;
        private readonly BugSimulationService _bugSimulationService;
        private readonly BugLifetimeService _bugLifetimeService;
        private readonly BugDeathService _bugDeathService;
        private readonly BugConsumptionService _bugConsumptionService;
        private readonly BugReproductionService _bugReproductionService;
        private CancellationToken _token;
        private bool _isSpawning;

        public BugService(
            BugsRegistry bugsRegistry,
            IBugFactory bugFactory,
            ISpawnPointProvider spawnPointProvider,
            ColonyRuleConfig colonyRuleConfig,
            BugSimulationService bugSimulationService,
            BugLifetimeService bugLifetimeService,
            BugDeathService bugDeathService,
            BugConsumptionService bugConsumptionService,
            BugReproductionService bugReproductionService)
        {
            _bugsRegistry = bugsRegistry ?? throw new ArgumentNullException(nameof(bugsRegistry));
            _bugFactory = bugFactory ?? throw new ArgumentNullException(nameof(bugFactory));
            _spawnPointProvider = spawnPointProvider ?? throw new ArgumentNullException(nameof(spawnPointProvider));
            _colonyRuleConfig = colonyRuleConfig ?? throw new ArgumentNullException(nameof(colonyRuleConfig));
            _bugSimulationService = bugSimulationService ?? throw new ArgumentNullException(nameof(bugSimulationService));
            _bugLifetimeService = bugLifetimeService ?? throw new ArgumentNullException(nameof(bugLifetimeService));
            _bugDeathService = bugDeathService ?? throw new ArgumentNullException(nameof(bugDeathService));
            _bugConsumptionService = bugConsumptionService ?? throw new ArgumentNullException(nameof(bugConsumptionService));
            _bugReproductionService = bugReproductionService ?? throw new ArgumentNullException(nameof(bugReproductionService));
        }

        public void StartColony(CancellationToken token)
        {
            _token = token;
            var startPosition = _spawnPointProvider.GetRandomBugSpawnPoint();
            var bug = _bugFactory.Spawn(BugType.Worker, startPosition);
            Activate(bug);
        }

        private void Activate(BugRuntime bug)
        {
            if (bug == null)
                return;

            _bugsRegistry.Register(bug);
            _bugSimulationService.Run(bug, OnTargetReached, _token);
            _bugLifetimeService.Run(bug, OnLifetimeExpired, _token);
        }

        private void Kill(BugRuntime bug, BugDeathReason reason)
        {
            if (!_bugDeathService.Kill(bug, reason))
                return;

            TryRespawnIfColonyEmpty();
        }

        private void OnTargetReached(BugRuntime bug, ITargetable target)
        {
            if (!_bugConsumptionService.TryConsume(bug, target))
                return;

            TryReproduce(bug);
            TryRespawnIfColonyEmpty();
        }

        private void OnLifetimeExpired(BugRuntime bug)
        {
            Kill(bug, BugDeathReason.LifetimeExpired);
        }

        private void TryReproduce(BugRuntime parent)
        {
            if (!_bugReproductionService.TryCreateOffspring(parent, out List<BugRuntime> offspring))
                return;

            if (!_bugDeathService.Kill(parent, BugDeathReason.Split))
                return;

            foreach (var child in offspring)
                Activate(child);
        }

        private void TryRespawnIfColonyEmpty()
        {
            if (!_colonyRuleConfig.RespawnWorkerIfColonyEmpty || _isSpawning || _bugsRegistry.AliveCount > 0)
                return;

            _isSpawning = true;

            var position = _spawnPointProvider.GetRandomBugSpawnPoint();
            var bug = _bugFactory.Spawn(BugType.Worker, position);
            Activate(bug);

            _isSpawning = false;
        }

        public void Dispose()
        {
            foreach (var bug in _bugsRegistry.AliveBugs.ToArray())
                bug.Dispose();

            _bugsRegistry.Clear();
        }
    }
}