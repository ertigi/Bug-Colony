using Project.Configs.Catalogs;
using Project.Configs.Colony;
using UnityEngine;
using Zenject;

namespace Project.Installers
{
    public class ConfigInstaller : MonoInstaller
    {
        [SerializeField] private ColonyRuleConfig _colonyRulesConfig;
        [SerializeField] private SpawnAreaConfig _spawnAreaConfig;
        [SerializeField] private BugConfigsCatalog _bugDefinitionsCatalog;
        [SerializeField] private ResourceConfigsCatalog _resourceDefinitionsCatalog;

        public override void InstallBindings()
        {
            Container.BindInstance(_colonyRulesConfig).AsSingle();
            Container.BindInstance(_spawnAreaConfig).AsSingle();
            Container.BindInstance(_bugDefinitionsCatalog).AsSingle();
            Container.BindInstance(_resourceDefinitionsCatalog).AsSingle();
        }
    }
}