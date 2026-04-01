using Project.Core.Domain.Bugs;
using System.Collections.Generic;

namespace Project.Core.Contracts
{
    public interface IReproductionStrategy
    {
        bool Reproduce(BugModel model);
    }
}