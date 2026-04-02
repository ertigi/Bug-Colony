using Project.Core.Contracts;
using Project.Core.Runtime;
using Project.Gameplay.World;

namespace Project.Gameplay.Bugs
{
    public class WorkerTargetingStrategy : ITargetingStrategy
    {
        private readonly WorldTargetService _targetService;

        public WorkerTargetingStrategy(WorldTargetService _targetService)
        {
            this._targetService = _targetService;
        }

        public ITargetable SelectTarget(BugRuntime self)
        {
            var targets = _targetService.GetConsumableTargetsForWorker();

            ITargetable bestTarget = null;
            var bestDistanceSqr = float.MaxValue;

            foreach (var target in targets)
            {
                if (target == null || !target.IsAvailable)
                    continue;

                var distanceSqr = (target.Position - self.Position).sqrMagnitude;
                if (distanceSqr >= bestDistanceSqr)
                    continue;

                bestDistanceSqr = distanceSqr;
                bestTarget = target;
            }

            return bestTarget;
        }
    }
}