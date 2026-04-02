using Project.Core.Contracts;
using Project.Core.Domain.Common;
using Project.Core.Domain.Resources;
using System;
using UnityEngine;

namespace Project.Core.Runtime
{
    public class ResourceRuntime : ITargetable
    {
        public ResourceModel Model { get; }
        public IResourceView View { get; }

        public TargetType TargetType => TargetType.Resource;
        public Vector3 Position => View.Position;
        public bool IsAvailable => Model.IsActive;

        public ResourceRuntime(ResourceModel model, IResourceView view)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            View = view ?? throw new ArgumentNullException(nameof(view));
        }

        public void SetPosition(Vector3 position)
        {
            View.SetPosition(position);
        }

        public bool TryConsume()
        {
            return Model.TryDeactivate();
        }

        public void Show()
        {
            View.Show();
        }

        public void Hide()
        {
            View.Hide();
        }

        public void ResetView()
        {
            View.ResetView();
        }
    }
}