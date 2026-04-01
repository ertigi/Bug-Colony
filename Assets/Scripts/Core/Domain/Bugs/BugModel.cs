using Project.Configs.Bugs;
using Project.Core.Domain.Bugs;
using Project.Core.Domain.Common;

namespace Project.Core.Domain.Bugs
{
    public class BugModel
    {
        public string Id { get; }
        public BugType Type { get; }
        public BugConfig Config { get; }

        public bool IsAlive { get; private set; }
        public int ConsumedCount { get; private set; }

        public BugModel(string id, BugType type, BugConfig config)
        {
            Id = id;
            Type = type;
            Config = config;
            IsAlive = true;
            ConsumedCount = 0;
        }

        public void AddConsumed(int amount = 1)
        {
            if (!IsAlive || amount <= 0)
                return;

            ConsumedCount += amount;
        }

        public bool TryMarkDead()
        {
            if (!IsAlive)
                return false;

            IsAlive = false;
            return true;
        }

        public void ResetConsumed()
        {
            ConsumedCount = 0;
        }
    }
}