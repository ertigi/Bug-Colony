using Project.Core.Contracts;
using Project.Core.Domain.Statistics;
using System;
using UniRx;

namespace Project.UI
{
    public class DeathCountersPresenter : IDisposable
    {
        private readonly DeathStatisticsModel _stats;
        private readonly DeathCountersView _view;
        private readonly CompositeDisposable _disposables = new();

        public DeathCountersPresenter(
            DeathStatisticsModel stats,
            DeathCountersView view)
        {
            _stats = stats ?? throw new ArgumentNullException(nameof(stats));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public void Initialize()
        {
            _stats.DeadWorkers
                .Subscribe(x => _view.SetWorkers(x))
                .AddTo(_disposables);

            _stats.DeadPredators
                .Subscribe(x => _view.SetPredators(x))
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}