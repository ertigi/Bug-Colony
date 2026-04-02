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
        private readonly BugService _bugService;

        public Bootstrapper(
            ResourceService resourceSpawnService,
            BugService bugActivationService)
        {
            _resourceSpawnService = resourceSpawnService ?? throw new ArgumentNullException(nameof(resourceSpawnService));
            _bugService = bugActivationService ?? throw new ArgumentNullException(nameof(bugActivationService));
        }

        public void Start(CancellationToken sceneToken)
        {
            _resourceSpawnService.Start(sceneToken);
            _bugService.StartColony(sceneToken);
        }
    }
}