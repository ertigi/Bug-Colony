using Project.Core.Domain.Bugs;
using UniRx;

namespace Project.Core.Domain.Statistics
{
    public class DeathStatisticsModel
    {
        public ReactiveProperty<int> DeadWorkers { get; } = new(0);
        public ReactiveProperty<int> DeadPredators { get; } = new(0);

        public void RegisterDeath(BugType bugType)
        {
            switch (bugType)
            {
                case BugType.Worker:
                    DeadWorkers.Value++;
                    break;

                case BugType.Predator:
                    DeadPredators.Value++;
                    break;
            }
        }
    }
}