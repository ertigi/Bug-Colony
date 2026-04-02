using Project.Core.Domain.Bugs;
using Project.Core.Domain.Statistics;
using System;

namespace Project.Core.Services
{
    public class DeathStatisticsService
    {
        private readonly DeathStatisticsModel _model;

        public DeathStatisticsService(DeathStatisticsModel model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public void RegisterDeath(BugType type)
        {
            _model.RegisterDeath(type);
        }
    }
}