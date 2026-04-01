using UnityEngine;


namespace Project.Configs.Colony
{
    [CreateAssetMenu(fileName = "ColonyRuleConfigSO", menuName = "Project/Configs/Colony Rules", order = 1)]
    public class ColonyRuleConfig : ScriptableObject
    {
        [field: Header("Reproduction")]
        [field: SerializeField, Min(1)] public int WorkerSplitThreshold { get; private set; } = 2;
        [field: SerializeField, Min(1)] public int PredatorSplitThreshold { get; private set; } = 3;


        [field: Header("Mutation")]
        [field: SerializeField] public int MutationAliveThreshold { get; private set; } = 10;
        [field: SerializeField] public float WorkerToPredatorMutationChance { get; private set; } = 0.1f;


        [field: Header("Lifetime")]
        [field: SerializeField] public float PredatorLifetimeSeconds { get; private set; } = 10f;


        [field: Header("Recovery")]
        [field: SerializeField] public bool RespawnWorkerIfColonyEmpty { get; private set; } = true;


        [field: Header("Resources")]
        [field: SerializeField] public float ResourceSpawnIntervalSeconds { get; private set; } = 1f;
        [field: SerializeField] public int MaxAliveResources { get; private set; } = 50;

    }
}