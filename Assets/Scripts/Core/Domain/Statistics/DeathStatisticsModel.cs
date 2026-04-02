using Project.Core.Domain.Bugs;
using UniRx;

namespace Project.Core.Domain.Statistics
{
    public class DeathStatisticsModel
    {
        public IReadOnlyReactiveProperty<int> DeadWorkers => _deadWorkers;
        public IReadOnlyReactiveProperty<int> DeadPredators => _deadPredators;

        private ReactiveProperty<int> _deadWorkers { get; } = new(0);
        private ReactiveProperty<int> _deadPredators { get; } = new(0);

        public void RegisterDeath(BugType bugType)
        {
            switch (bugType)
            {
                case BugType.Worker:
                    _deadWorkers.Value++;
                    break;

                case BugType.Predator:
                    _deadPredators.Value++;
                    break;
            }
        }
    }
}