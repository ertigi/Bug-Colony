using Project.Core.Domain.Bugs;
using Project.Core.Runtime;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Project.Core.Domain.Colony
{
    public class BugsRegistry
    {
        private readonly Dictionary<int, BugRuntime> _aliveBugs = new();

        public int AliveCount => _aliveBugs.Count;

        public IReadOnlyCollection<BugRuntime> AliveBugs => _aliveBugs.Values;

        public bool Register(BugRuntime bug)
        {
            if (bug == null)
                throw new ArgumentNullException(nameof(bug));

            if (!_aliveBugs.TryAdd(bug.Model.Id, bug))
                return false;

            return true;
        }

        public bool Unregister(BugRuntime bug)
        {
            if (bug == null)
                throw new ArgumentNullException(nameof(bug));

            return _aliveBugs.Remove(bug.Model.Id); ;
        }

        public void Clear()
        {
            _aliveBugs.Clear();
        }
    }
}