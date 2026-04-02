using Project.Core.Domain.Bugs;
using Project.Core.Runtime;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Project.Core.Domain.Colony
{
    public class ColonyRegistry
    {
        private readonly Dictionary<int, BugRuntime> _aliveBugs = new();

        public ReactiveProperty<int> AliveCount { get; } = new(0);

        public IReadOnlyCollection<BugRuntime> AliveBugs => _aliveBugs.Values;

        public bool Register(BugRuntime bug)
        {
            if (bug == null)
            {
                throw new ArgumentNullException(nameof(bug));
            }

            var key = bug.Model.Id;
            if (!_aliveBugs.TryAdd(key, bug))
            {
                return false;
            }

            AliveCount.Value = _aliveBugs.Count;
            return true;
        }

        public bool Unregister(BugRuntime bug)
        {
            if (bug == null)
            {
                throw new ArgumentNullException(nameof(bug));
            }

            var removed = _aliveBugs.Remove(bug.Model.Id);
            if (removed)
            {
                AliveCount.Value = _aliveBugs.Count;
            }

            return removed;
        }

        public bool Contains(BugRuntime bug)
        {
            if (bug == null)
            {
                return false;
            }

            return _aliveBugs.ContainsKey(bug.Model.Id);
        }

        public int GetAliveCount(BugType bugType)
        {
            var count = 0;

            foreach (var bug in _aliveBugs.Values)
            {
                if (bug.Model.Type == bugType && bug.Model.IsAlive)
                {
                    count++;
                }
            }

            return count;
        }

        public void Clear()
        {
            _aliveBugs.Clear();
            AliveCount.Value = 0;
        }
    }
}