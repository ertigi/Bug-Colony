using Project.Core.Contracts;
using Project.Core.Factories;
using Zenject;

namespace Project.Installers
{
    public class FactoryInstaller : Installer<FactoryInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<BugBehaviourProfileFactory>().AsSingle();
            Container.Bind<IBugFactory>().To<BugFactory>().AsSingle();
            Container.Bind<IResourceFactory>().To<ResourceFactory>().AsSingle();
        }
    }
}