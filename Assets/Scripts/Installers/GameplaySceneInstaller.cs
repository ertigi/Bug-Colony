using Project.Core.Bootstrap;
using UnityEngine;
using Zenject;

namespace Project.Installers
{
    public class GameplaySceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            PoolInstaller.Install(Container);
            FactoryInstaller.Install(Container);
            ServiceInstaller.Install(Container);

            Container.Bind<Bootstrapper>().AsSingle();
            Container.Bind<SimulationEntryPoint>().AsSingle();
        }

        public override void Start()
        {
            Container.Resolve<SimulationEntryPoint>().Run();
        }
    }
}