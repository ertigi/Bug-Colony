using Project.Core.Contracts;
using Project.Core.Runtime;
using Project.Gameplay.World;

namespace Project.Gameplay.Bugs
{
    public sealed class PredatorTargetingStrategy : ITargetingStrategy
    {
        private readonly WorldTargetService _targetService;

        public PredatorTargetingStrategy(WorldTargetService targetService)
        {
            _targetService = targetService;
        }

        public ITargetable SelectTarget(BugRuntime self)
        {
            var targets = _targetService.GetConsumableTargetsForPredator(self);

            ITargetable bestTarget = null;
            var bestDistanceSqr = float.MaxValue;

            foreach (var target in targets)
            {
                if (target == null || !target.IsAvailable)
                    continue;

                var distanceSqr = (target.Position - self.Position).sqrMagnitude;
                if (distanceSqr >= bestDistanceSqr)
                    continue;

                bestTarget = target;
                bestDistanceSqr = distanceSqr;
            }

            return bestTarget;
        }
    }
}