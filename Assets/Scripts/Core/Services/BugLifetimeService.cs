using Project.Core.Contracts;
using Project.Core.Runtime;
using System;
using Unity.VisualScripting;

namespace Project.Core.Application.Services
{
    public sealed class BugLifetimeService
    {
        private readonly IBugKillService _killService;

        public BugLifetimeService(IBugKillService lifecycleService)
        {
            _killService = lifecycleService ?? throw new ArgumentNullException(nameof(lifecycleService));
        }
/*
        public void Start(BugRuntime bug)
        {
            if (bug == null)
            {
                return;
            }

            var policy = bug.LifetimePolicy;
            if (policy == null || !policy.HasLifetime)
            {
                return;
            }

            RunLifetime(bug, policy.GetLifetimeSeconds()).Forget();
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
                    _lifecycleService.Kill(bug);
                }
            }
            catch (OperationCanceledException)
            {
                // ignored
            }
        }
*/
    }
}