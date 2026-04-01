using UnityEngine;

namespace Project.Core.Contracts
{
    public interface IResourceView
    {
        Vector3 Position { get; }
        void SetPosition(Vector3 position);
        void Show();
        void Hide();
        void ResetView();
    }
}