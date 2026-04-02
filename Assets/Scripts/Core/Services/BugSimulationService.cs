using Cysharp.Threading.Tasks;
using Project.Core.Contracts;
using Project.Core.Runtime;
using System;
using System.Threading;
using UnityEngine;

namespace Project.Core.Services
{
    public class BugSimulationService
    {
        public void Run(BugRuntime bug, Action<BugRuntime, ITargetable> onTargetReached, CancellationToken token)
        {
            if (bug == null)
                return;

            RunLoop(bug, onTargetReached, token).Forget();
        }

        private async UniTaskVoid RunLoop(BugRuntime bug, Action<BugRuntime, ITargetable> onTargetReached, CancellationToken sceneToken)
        {
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(bug.LifetimeToken, sceneToken);
            var token = linkedCts.Token;

            try
            {
                while (!token.IsCancellationRequested && bug.Model.IsAlive)
                {
                    if (bug.CurrentTarget == null || !bug.CurrentTarget.IsAvailable)
                        bug.SetTarget(bug.TargetingStrategy.SelectTarget(bug));

                    if (bug.CurrentTarget == null)
                    {
                        await UniTask.Yield(PlayerLoopTiming.Update, token);
                        continue;
                    }

                    bug.MoveTowardsCurrentTarget(Time.deltaTime);

                    if (bug.HasReachedCurrentTarget())
                    {
                        if (token.IsCancellationRequested)
                            return;

                        onTargetReached?.Invoke(bug, bug.CurrentTarget);
                    }

                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
    }

}