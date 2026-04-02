using Project.Core.Runtime;

namespace Project.Core.Contracts
{
    public interface IBugKillService
    {
        void Kill(BugRuntime bug);
    }
}