using Project.Core.Domain.Bugs;
using Project.Gameplay.Bugs;
using UnityEngine;

namespace Project.Configs.Bugs
{
    [CreateAssetMenu(fileName = "BugConfigSO", menuName = "Project/Configs/Bug Config", order = 1)]
    public class BugConfig : ScriptableObject
    {
        [field: SerializeField] public BugType Type { get; private set; }
        [field: SerializeField] public BugView Prefab { get; private set; }
        [field: SerializeField] public float MoveSpeed { get; private set; } = 1f;
        [field: SerializeField] public float EatDistance { get; private set; } = .5f;
        [field: SerializeField] public int NutritionValue { get; private set; } = 1;
    }
}
