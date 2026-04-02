using Project.Core.Domain.Colony;
using Project.Core.Runtime;
using System.Collections.Generic;

namespace Project.Core.Contracts
{
    public interface IMutationStrategy
    {
        void MutateOffspring(List<OffspringDescriptor> offspring, BugRuntime parent, ColonyRegistry colonyRegistry);
    }
}