using Project.Core.Contracts;
using Project.Core.Domain.Bugs;
using System;

namespace Project.Core.Factories
{
    public class BugBehaviourFactory
    {
        private readonly Func<BugType, IBugMover> _moverFactory;
        private readonly Func<BugType, ITargetingStrategy> _targetingFactory;
        private readonly Func<BugType, IFeedingStrategy> _feedingFactory;
        private readonly Func<BugType, IReproductionStrategy> _reproductionFactory;
        private readonly Func<BugType, IMutationStrategy> _mutationFactory;
        private readonly Func<BugType, ILifetimeStrategy> _lifetimeFactory;

        public BugBehaviourFactory(
            Func<BugType, IBugMover> moverFactory,
            Func<BugType, ITargetingStrategy> targetingFactory,
            Func<BugType, IFeedingStrategy> feedingFactory,
            Func<BugType, IReproductionStrategy> reproductionFactory,
            Func<BugType, IMutationStrategy> mutationFactory,
            Func<BugType, ILifetimeStrategy> lifetimeFactory)
        {
            _moverFactory = moverFactory ?? throw new ArgumentNullException(nameof(moverFactory));
            _targetingFactory = targetingFactory ?? throw new ArgumentNullException(nameof(targetingFactory));
            _feedingFactory = feedingFactory ?? throw new ArgumentNullException(nameof(feedingFactory));
            _reproductionFactory = reproductionFactory ?? throw new ArgumentNullException(nameof(reproductionFactory));
            _mutationFactory = mutationFactory ?? throw new ArgumentNullException(nameof(mutationFactory));
            _lifetimeFactory = lifetimeFactory ?? throw new ArgumentNullException(nameof(lifetimeFactory));
        }

        public BugBehaviourProfile Create(BugType bugType)
        {
            return new BugBehaviourProfile(
                _moverFactory.Invoke(bugType),
                _targetingFactory.Invoke(bugType),
                _feedingFactory.Invoke(bugType),
                _reproductionFactory.Invoke(bugType),
                _mutationFactory.Invoke(bugType),
                _lifetimeFactory.Invoke(bugType));
        }
    }

    public struct BugBehaviourProfile
    {
        public IBugMover Mover { get; }
        public ITargetingStrategy TargetingStrategy { get; }
        public IFeedingStrategy FeedingPolicy { get; }
        public IReproductionStrategy ReproductionPolicy { get; }
        public IMutationStrategy MutationPolicy { get; }
        public ILifetimeStrategy LifetimePolicy { get; }

        public BugBehaviourProfile(
            IBugMover mover,
            ITargetingStrategy targetingStrategy,
            IFeedingStrategy feedingPolicy,
            IReproductionStrategy reproductionPolicy,
            IMutationStrategy mutationPolicy,
            ILifetimeStrategy lifetimePolicy)
        {
            Mover = mover;
            TargetingStrategy = targetingStrategy;
            FeedingPolicy = feedingPolicy;
            ReproductionPolicy = reproductionPolicy;
            MutationPolicy = mutationPolicy;
            LifetimePolicy = lifetimePolicy;
        }
    }
}