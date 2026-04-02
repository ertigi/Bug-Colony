using Project.UI;
using UnityEngine;
using Zenject;

namespace Project.Installers
{
    public class UiInstaller : MonoInstaller
    {
        [SerializeField] private DeathCountersView _deathCountersView;

        public override void InstallBindings()
        {
            Container.BindInstance(_deathCountersView).AsSingle();
            Container.Bind<DeathCountersPresenter>().AsSingle().NonLazy();
        }

        public override void Start()
        {
            Container.Resolve<DeathCountersPresenter>().Initialize();
        }
    }
}