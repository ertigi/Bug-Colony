using Project.Core.Contracts;
using Project.Core.Domain.Common;
using UnityEngine;

namespace Project.Gameplay.Bugs
{
    public class BugView : MonoBehaviour, IBugView
    {
        [SerializeField] private Transform _transform;

        public Vector3 Position => _transform != null ? _transform.position : Vector3.zero;

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