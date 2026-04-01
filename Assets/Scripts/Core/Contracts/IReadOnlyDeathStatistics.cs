using UniRx;

namespace Project.Core.Contracts
{
    public interface IReadOnlyDeathStatistics
    {
        IReadOnlyReactiveProperty<int> DeadWorkers { get; }
        IReadOnlyReactiveProperty<int> DeadPredators { get; }
    }
}