using Project.Configs.Colony;
using Project.Core.Contracts;
using Project.Core.Domain.Bugs;
using Project.Gameplay.Bugs;
using Project.Gameplay.World;
using System;

namespace Project.Core.Factories
{
    public class BugBehaviourProfileFactory
    {
        private readonly TargetService _queryService;
        private readonly ColonyRuleConfig _rulesConfig;

        public BugBehaviourProfileFactory(
            TargetService queryService,
            ColonyRuleConfig rulesConfig)
        {
            _queryService = queryService ?? throw new ArgumentNullException(nameof(queryService));
            _rulesConfig = rulesConfig ?? throw new ArgumentNullException(nameof(rulesConfig));
        }

        public BugBehaviourProfile Create(BugType bugType)
        {
            return bugType switch
            {
                BugType.Worker => CreateWorkerProfile(),
                BugType.Predator => CreatePredatorProfile(),
                _ => throw new NotSupportedException($"{bugType} not supported.")
            };
        }

        private BugBehaviourProfile CreateWorkerProfile()
        {
            return new BugBehaviourProfile(
                new TransformBugMover(),
                new BugTargetingStrategy(_queryService, BugType.Worker),
                new WorkerFeedingStrategy(),
                new BugReproductionStrategy(_rulesConfig.WorkerSplitThreshold, _rulesConfig.WorkerSplitOffspring, BugType.Worker),
                new WorkerToPredatorMutationStrategy(_rulesConfig),
                new NoLifetimeStrategy());
        }

        private BugBehaviourProfile CreatePredatorProfile()
        {
            return new BugBehaviourProfile(
                new TransformBugMover(),
                new BugTargetingStrategy(_queryService, BugType.Predator),
                new PredatorFeedingStrategy(),
                new BugReproductionStrategy(_rulesConfig.PredatorSplitThreshold, _rulesConfig.PredatorSplitOffspring, BugType.Predator),
                new NoMutationStrategy(),
                new FixedLifetimeStrategy(_rulesConfig.PredatorLifetimeSeconds));
        }
    }
    public struct BugBehaviourProfile
    {
        public IBugMover Mover { get; }
        public ITargetingStrategy TargetingStrategy { get; }
        public IFeedingStrategy FeedingStrategy { get; }
        public IReproductionStrategy ReproductionStrategy { get; }
        public IMutationStrategy MutationStrategy { get; }
        public ILifetimeStrategy LifetimeStrategy { get; }

        public BugBehaviourProfile(
            IBugMover mover,
            ITargetingStrategy targetingStrategy,
            IFeedingStrategy feedingStrategy,
            IReproductionStrategy reproductionStrategy,
            IMutationStrategy mutationStrategy,
            ILifetimeStrategy lifetimeStrategy)
        {
            Mover = mover;
            TargetingStrategy = targetingStrategy;
            FeedingStrategy = feedingStrategy;
            ReproductionStrategy = reproductionStrategy;
            MutationStrategy = mutationStrategy;
            LifetimeStrategy = lifetimeStrategy;
        }
    }
}