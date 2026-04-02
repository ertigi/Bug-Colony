using Project.Core.Contracts;
using Project.Core.Domain.Bugs;
using Project.Core.Runtime;
using Project.Gameplay.World;

namespace Project.Gameplay.Bugs
{
    public class PredatorTargetingStrategy : ITargetingStrategy
    {
        private readonly TargetService _targetService;
        private readonly BugType _bugType;

        public PredatorTargetingStrategy(TargetService targetService, BugType bugType)
        {
            _targetService = targetService;
            _bugType = bugType;
        }

        public ITargetable SelectTarget(BugRuntime self)
        {
            return _targetService.GetNearestTargets(self, _bugType);
        }
    }
}