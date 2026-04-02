using Project.Core.Contracts;

namespace Project.Gameplay.Bugs
{
    public class FixedLifetimePolicy : ILifetimeStrategy
    {
        private readonly float _lifetimeSeconds;

        public FixedLifetimePolicy(float lifetimeSeconds)
        {
            _lifetimeSeconds = lifetimeSeconds;
        }

        public bool HasLifetime => _lifetimeSeconds > 0f;

        public float GetLifetimeSeconds()
        {
            return _lifetimeSeconds;
        }
    }
}