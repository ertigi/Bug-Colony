using Project.Configs.Resources;
using Project.Core.Domain.Common;
using Project.Core.Domain.Resources;

namespace Project.Core.Domain.Resources
{
    public class ResourceModel
    {
        public int Id { get; }
        public ResourceType Type { get; }
        public int NutritionValue { get; }
        public ResourceConfig ResourceConfig { get; }

        public bool IsActive { get; private set; }

        public ResourceModel(int id, ResourceConfig resourceConfig)
        {
            Id = id;
            ResourceConfig = resourceConfig;
            Type = resourceConfig.Type;
            NutritionValue = resourceConfig.NutritionValue;
            IsActive = true;
        }

        public bool TryDeactivate()
        {
            if (!IsActive)
                return false;

            IsActive = false;
            return true;
        }
    }
}