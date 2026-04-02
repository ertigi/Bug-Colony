using UnityEngine;

namespace Project.Configs.Colony
{
    [CreateAssetMenu(fileName = "SpawnAreaConfigSO", menuName = "Project/Configs/Spawn Area")]
    public class SpawnAreaConfig : ScriptableObject
    {
        [field: SerializeField] public Vector3 Center { get; private set; } = Vector3.zero;
        [field: SerializeField] public Vector3 Size { get; private set; } = new Vector3(20f, 0f, 20f);
        [field: SerializeField] public float SplitOffset { get; private set; } = 2f;

        public Bounds GetBounds()
        {
            return new Bounds(Center, Size);
        }
    }
}