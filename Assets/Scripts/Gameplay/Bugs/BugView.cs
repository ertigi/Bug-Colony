using Project.Core.Contracts;
using UnityEngine;

namespace Project.Gameplay.Bugs
{
    public class BugView : MonoBehaviour, IBugView
    {
        [SerializeField] private Transform _transform;

        public Vector3 Position => _transform.position;

        private void Awake()
        {
            if (_transform == null)
            {
                _transform = transform;
            }
        }

        public void SetPosition(Vector3 position)
        {
            _transform.position = position;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ResetView()
        {
            _transform.localScale = Vector3.one;
            _transform.rotation = Quaternion.identity;
        }
    }
}