using Project.Core.Contracts;
using Project.Core.Domain.Colony;
using Project.Core.Runtime;
using System.Collections.Generic;

namespace Project.Gameplay.Bugs
{
    public class NoMutationStrategy : IMutationStrategy
    {
        public void MutateOffspring(List<OffspringDescriptor> offspring, BugRuntime parent, BugsRegistry bugsRegistry)
        {

        }
    }
}