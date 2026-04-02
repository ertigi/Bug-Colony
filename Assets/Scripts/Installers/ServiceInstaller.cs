using Project.Core.Domain.Colony;
using Project.Core.Domain.Statistics;
using Project.Core.Services;
using Project.Gameplay.World;
using Zenject;

namespace Project.Installers
{
    public class ServiceInstaller : Installer<ServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<BugsRegistry>().AsSingle();
            Container.Bind<ResourceRegistry>().AsSingle();
            Container.Bind<DeathStatisticsModel>().AsSingle();

            Container.Bind<TargetService>().AsSingle();

            Container.BindInterfacesAndSelfTo<SceneBoundsSpawnPointProvider>().AsSingle();

            Container.Bind<DeathStatisticsService>().AsSingle();

            Container.Bind<BugSimulationService>().AsSingle();
            Container.Bind<BugLifetimeService>().AsSingle();
            Container.Bind<BugConsumptionService>().AsSingle();
            Container.Bind<BugReproductionService>().AsSingle();
            Container.Bind<BugDeathService>().AsSingle();
            Container.Bind<BugService>().AsSingle();

            Container.Bind<ResourceService>().AsSingle();
        }
    }
}