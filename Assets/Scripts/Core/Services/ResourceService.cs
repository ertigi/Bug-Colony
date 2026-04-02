using Cysharp.Threading.Tasks;
using Project.Configs.Colony;
using Project.Core.Contracts;
using Project.Core.Domain.Resources;
using Project.Core.Runtime;
using Project.Gameplay.Resources.Views;
using Project.Gameplay.World;
using System;
using System.Threading;

namespace Project.Core.Services
{
    public class ResourceService : IDisposable
    {
        private readonly IResourceFactory _resourceFactory;
        private readonly ISpawnPointProvider _spawnPointProvider;
        private readonly ResourceRegistry _resourceRegistry;
        private readonly ColonyRuleConfig _rulesConfig;
        private readonly ResourceViewPool _resourceViewPool;

        public ResourceService(
            IResourceFactory resourceFactory,
            ISpawnPointProvider spawnPointProvider,
            ResourceRegistry resourceRegistry,
            ColonyRuleConfig rulesConfig,
            ResourceViewPool resourceViewPool)
        {
            _resourceFactory = resourceFactory ?? throw new ArgumentNullException(nameof(resourceFactory));
            _spawnPointProvider = spawnPointProvider ?? throw new ArgumentNullException(nameof(spawnPointProvider));
            _resourceRegistry = resourceRegistry ?? throw new ArgumentNullException(nameof(resourceRegistry));
            _rulesConfig = rulesConfig ?? throw new ArgumentNullException(nameof(rulesConfig));
            _resourceViewPool = resourceViewPool ?? throw new ArgumentNullException(nameof(resourceViewPool));
        }

        public void Start(CancellationToken token)
        {
            SpawnLoop(token).Forget();
        }

        public void Release(ResourceRuntime resource)
        {
            if (resource == null)
                return;

            _resourceRegistry.Unregister(resource);
            _resourceViewPool.Push(resource.Model.Type, (ResourceView)resource.View);
        }

        private ResourceRuntime Spawn(ResourceType type)
        {
            var position = _spawnPointProvider.GetRandomResourceSpawnPoint();
            var resource = _resourceFactory.Spawn(type, position);
            _resourceRegistry.Register(resource);
            return resource;
        }

        private async UniTaskVoid SpawnLoop(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await UniTask.Delay(
                        TimeSpan.FromSeconds(_rulesConfig.ResourceSpawnIntervalSeconds),
                        cancellationToken: token);

                    if (_resourceRegistry.Count() >= _rulesConfig.MaxAliveResources)
                        continue;

                    Spawn(ResourceType.Food);
                }
            }
            catch (OperationCanceledException)
            {

            }
        }

        public void Dispose()
        {
            _resourceViewPool.Clear();
        }
    }
}