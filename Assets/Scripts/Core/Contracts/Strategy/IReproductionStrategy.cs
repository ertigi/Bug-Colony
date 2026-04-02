using Project.Core.Domain.Bugs;
using System.Collections.Generic;

namespace Project.Core.Contracts
{
    public interface IReproductionStrategy
    {
        BugType GetBaseOffspringType();
        int GetOffspringCount();
        bool ShouldReproduce(BugModel model);
    }
}