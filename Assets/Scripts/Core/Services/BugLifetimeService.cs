using Cysharp.Threading.Tasks;
using Project.Core.Runtime;
using System;

namespace Project.Core.Services
{
    public class BugLifetimeService
    {
        public void Run(BugRuntime bug, Action<BugRuntime> onExpired)
        {
            if (bug == null)
                return;

            var Strategy = bug.LifetimeStrategy;
            if (Strategy == null || !Strategy.HasLifetime)
                return;

            RunLifetime(bug, Strategy.GetLifetimeSeconds(), onExpired).Forget();
        }

        private async UniTaskVoid RunLifetime(BugRuntime bug, float lifetimeSeconds, Action<BugRuntime> onExpired)
        {
            try
            {
                await UniTask.Delay(
                    TimeSpan.FromSeconds(lifetimeSeconds),
                    cancellationToken: bug.LifetimeToken);

                if (bug.Model.IsAlive)
                    onExpired?.Invoke(bug);
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}