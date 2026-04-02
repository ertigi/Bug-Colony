using Cysharp.Threading.Tasks;
using Project.Core.Runtime;
using System;
using System.Threading;

namespace Project.Core.Services
{
    public class BugLifetimeService
    {
        public void Run(BugRuntime bug, Action<BugRuntime> onExpired, CancellationToken token)
        {
            if (bug == null)
                return;

            var Strategy = bug.LifetimeStrategy;
            if (Strategy == null || !Strategy.HasLifetime)
                return;

            RunLifetime(bug, Strategy.GetLifetimeSeconds(), onExpired, token).Forget();
        }

        private async UniTaskVoid RunLifetime(BugRuntime bug, float lifetimeSeconds, Action<BugRuntime> onExpired, CancellationToken sceneToken)
        {
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(bug.LifetimeToken, sceneToken);
            var token = linkedCts.Token;

            try
            {
                await UniTask.Delay(
                    TimeSpan.FromSeconds(lifetimeSeconds),
                    cancellationToken: token);

                if (bug.Model.IsAlive)
                    onExpired?.Invoke(bug);
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}