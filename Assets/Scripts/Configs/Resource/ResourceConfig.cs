using Project.Core.Domain.Resources;
using UnityEngine;

namespace Project.Configs.Resources
{
    [CreateAssetMenu(fileName = "ResourceConfigSO", menuName = "Project/Configs/Resource Config", order = 1)]
    public class ResourceConfig : ScriptableObject
    {
        [field: SerializeField] public ResourceType Type { get; private set; }
        [field: SerializeField] public GameObject Prefab { get; private set; }
        [field: SerializeField] public int NutritionValue { get; private set; } = 1;
        [field: SerializeField] public float InteractionRadius { get; private set; } = .5f;
    }
}