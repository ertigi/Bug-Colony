namespace Project.Core.Contracts
{
    public interface ILifetimeStrategy
    {
        bool HasLifetime { get; }
        float GetLifetimeSeconds();
    }
}