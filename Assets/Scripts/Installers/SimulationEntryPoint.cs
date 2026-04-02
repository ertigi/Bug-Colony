using Project.Core.Bootstrap;
using System;
using System.Threading;

namespace Project.Installers
{
    public class SimulationEntryPoint : IDisposable
    {
        private readonly Bootstrapper _bootstrapper;
        private readonly CancellationTokenSource _sceneCts = new();

        public SimulationEntryPoint(Bootstrapper bootstrapper)
        {
            _bootstrapper = bootstrapper ?? throw new ArgumentNullException(nameof(bootstrapper));
        }

        public void Run()
        {
            _bootstrapper.Start(_sceneCts.Token);
        }

        public void Dispose()
        {
            if (!_sceneCts.IsCancellationRequested)
            {
                _sceneCts.Cancel();
            }

            _sceneCts.Dispose();
        }
    }
}