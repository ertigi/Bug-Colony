using TMPro;
using UnityEngine;

namespace Project.UI
{
    public class DeathCountersView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _workersText;
        [SerializeField] private TMP_Text _predatorsText;

        public void SetWorkers(int value)
        {
            if (_workersText != null)
            {
                _workersText.text = $"Workers Dead: {value}";
            }
        }

        public void SetPredators(int value)
        {
            if (_predatorsText != null)
            {
                _predatorsText.text = $"Predators Dead: {value}";
            }
        }
    }
}