using Project.Core.Contracts;
using Project.Core.Domain.Bugs;
using Project.Core.Services;
using System;
using System.Threading;

namespace Project.Core.Bootstrap
{
    public class Bootstrapper
    {
        private readonly ResourceService _resourceSpawnService;
        private readonly IBugFactory _bugFactory;
        private readonly BugService _bugService;
        private readonly ISpawnPointProvider _spawnPointProvider;

        public Bootstrapper(
            ResourceService resourceSpawnService,
            IBugFactory bugFactory,
            BugService bugActivationService,
            ISpawnPointProvider spawnPointProvider)
        {
            _resourceSpawnService = resourceSpawnService ?? throw new ArgumentNullException(nameof(resourceSpawnService));
            _bugFactory = bugFactory ?? throw new ArgumentNullException(nameof(bugFactory));
            _bugService = bugActivationService ?? throw new ArgumentNullException(nameof(bugActivationService));
            _spawnPointProvider = spawnPointProvider ?? throw new ArgumentNullException(nameof(spawnPointProvider));
        }

        public void Start(CancellationToken sceneToken)
        {
            _resourceSpawnService.Start(sceneToken);

            var startPosition = _spawnPointProvider.GetRandomBugSpawnPoint();
            var bug = _bugFactory.Spawn(BugType.Worker, startPosition);

            _bugService.Activate(bug);
        }
    }
}