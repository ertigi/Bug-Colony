using Project.Core.Contracts;
using Project.Core.Domain.Bugs;

namespace Project.Gameplay.Bugs
{
    public class BugReproductionStrategy : IReproductionStrategy
    {
        private readonly int _threshold;
        private readonly int _offspringCount;
        private readonly BugType _offspringType;

        public BugReproductionStrategy(int threshold, int offspringCount, BugType offspringType)
        {
            _threshold = threshold;
            _offspringCount = offspringCount;
            _offspringType = offspringType;
        }

        public bool ShouldReproduce(BugModel model)
        {
            return model != null &&
                   model.IsAlive &&
                   model.ConsumedCount >= _threshold;
        }

        public int GetOffspringCount()
        {
            return _offspringCount;
        }

        public BugType GetBaseOffspringType()
        {
            return _offspringType;
        }
    }
}