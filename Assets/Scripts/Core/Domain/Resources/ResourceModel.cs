using Project.Core.Domain.Common;
using Project.Core.Domain.Resources;

namespace Project.Core.Domain.Resources
{
    public class ResourceModel
    {
        public string Id { get; }
        public ResourceType Type { get; }
        public int NutritionValue { get; }

        public bool IsActive { get; private set; }

        public ResourceModel(string id, ResourceType type, int nutritionValue)
        {
            Id = id;
            Type = type;
            NutritionValue = nutritionValue;
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