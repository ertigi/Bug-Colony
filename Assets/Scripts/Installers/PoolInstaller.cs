using Project.Gameplay.Bugs;
using Project.Gameplay.Resources.Views;
using Zenject;

namespace Project.Installers
{
    public class PoolInstaller : Installer<PoolInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<BugViewPool>().AsSingle();
            Container.Bind<ResourceViewPool>().AsSingle();
        }
    }
}