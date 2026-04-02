using Project.Core.Contracts;
using Project.Core.Domain.Bugs;
using Project.Core.Domain.Common;
using System;
using System.Threading;
using UniRx;
using UnityEngine;

namespace Project.Core.Runtime
{
    public class BugRuntime : ITargetable, IDisposable
    {
        private readonly CancellationTokenSource _lifetimeCts = new();

        public BugModel Model { get; }
        public IBugView View { get; }
        public IBugMover Mover { get; }
        public ITargetingStrategy TargetingStrategy { get; }
        public IFeedingStrategy FeedingPolicy { get; }
        public IReproductionStrategy ReproductionPolicy { get; }
        public IMutationStrategy MutationPolicy { get; }
        public ILifetimeStrategy LifetimePolicy { get; }

        public CompositeDisposable Disposables { get; } = new();
        public ITargetable CurrentTarget { get; private set; }

        public TargetType TargetType => TargetType.Bug;
        public Vector3 Position => View.Position;
        public bool IsAvailable => Model.IsAlive;
        public CancellationToken LifetimeToken => _lifetimeCts.Token;

        public BugRuntime(
            BugModel model,
            IBugView view,
            IBugMover mover,
            ITargetingStrategy targetingStrategy,
            IFeedingStrategy feedingPolicy,
            IReproductionStrategy reproductionPolicy,
            IMutationStrategy mutationPolicy,
            ILifetimeStrategy lifetimePolicy)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            View = view ?? throw new ArgumentNullException(nameof(view));
            Mover = mover ?? throw new ArgumentNullException(nameof(mover));
            TargetingStrategy = targetingStrategy ?? throw new ArgumentNullException(nameof(targetingStrategy));
            FeedingPolicy = feedingPolicy ?? throw new ArgumentNullException(nameof(feedingPolicy));
            ReproductionPolicy = reproductionPolicy ?? throw new ArgumentNullException(nameof(reproductionPolicy));
            MutationPolicy = mutationPolicy ?? throw new ArgumentNullException(nameof(mutationPolicy));
            LifetimePolicy = lifetimePolicy ?? throw new ArgumentNullException(nameof(lifetimePolicy));
        }

        public void SetPosition(Vector3 position)
        {
            View.SetPosition(position);
        }

        public void SetTarget(ITargetable target)
        {
            CurrentTarget = target;
        }

        public void ClearTarget()
        {
            CurrentTarget = null;
        }

        public void MoveTowardsCurrentTarget(float deltaTime)
        {
            if (CurrentTarget == null)
            {
                return;
            }

            Mover.MoveTowards(View, CurrentTarget.Position, Model.Config.MoveSpeed, deltaTime);
        }

        public bool HasReachedCurrentTarget()
        {
            return CurrentTarget != null &&
                   Mover.HasReached(View, CurrentTarget.Position, Model.Config.EatDistance);
        }

        public bool TryMarkDead()
        {
            return Model.TryMarkDead();
        }

        public void Show()
        {
            View.Show();
        }

        public void Hide()
        {
            View.Hide();
        }

        public void ResetView()
        {
            View.ResetView();
        }

        public void Dispose()
        {
            if (!_lifetimeCts.IsCancellationRequested)
                _lifetimeCts.Cancel();

            Disposables.Dispose();
            CurrentTarget = null;
        }
    }
}