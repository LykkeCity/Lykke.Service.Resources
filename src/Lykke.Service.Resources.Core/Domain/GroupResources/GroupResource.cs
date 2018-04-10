using System.Collections.Generic;

namespace Lykke.Service.Resources.Core.Domain.GroupResources
{
    public class GroupResource : IGroupResource
    {
        public string Name { get; set; }
        public IReadOnlyCollection<GroupItem> Value { get; set; }
        
        public static IGroupResource Create(IGroupResource src)
        {
            return new GroupResource
            {
                Name = src.Name,
                Value = src.Value
            };
        }
    }
}
