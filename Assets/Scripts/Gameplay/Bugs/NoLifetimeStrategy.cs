using Project.Core.Contracts;

namespace Project.Gameplay.Bugs
{
    public class NoLifetimeStrategy : ILifetimeStrategy
    {
        public bool HasLifetime => false;

        public float GetLifetimeSeconds()
        {
            return 0f;
        }
    }
}