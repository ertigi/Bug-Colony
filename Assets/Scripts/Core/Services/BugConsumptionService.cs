using Project.Core.Contracts;
using Project.Core.Domain.Common;
using Project.Core.Runtime;
using System;

namespace Project.Core.Services
{
    public class BugConsumptionService
    {
        private readonly ResourceService _resourceService;
        private readonly BugDeathService _bugDeathService;

        public BugConsumptionService(
            ResourceService resourceService,
            BugDeathService bugDeathService)
        {
            _resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
            _bugDeathService = bugDeathService ?? throw new ArgumentNullException(nameof(bugDeathService));
        }

        public bool TryConsume(BugRuntime self, ITargetable target)
        {
            if (self == null || target == null || !self.Model.IsAlive || !target.IsAvailable)
                return false;

            if (!self.FeedingStrategy.CanConsume(self, target))
                return false;

            if (target is ResourceRuntime resource)
            {
                if (!resource.TryConsume())
                    return false;

                _resourceService.Release(resource);
            }
            else if (target is BugRuntime bug)
            {
                if (bug == self || !bug.Model.IsAlive)
                    return false;

                if (!_bugDeathService.Kill(bug, BugDeathReason.Consumed))
                    return false;
            }
            else
            {
                return false;
            }

            var nutrition = self.FeedingStrategy.GetNutritionValue(target);
            self.Model.AddConsumed(nutrition);
            self.ClearTarget();

            return true;
        }
    }
}