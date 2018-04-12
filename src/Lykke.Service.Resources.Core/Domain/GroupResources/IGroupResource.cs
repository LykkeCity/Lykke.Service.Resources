using System.Collections.Generic;

namespace Lykke.Service.Resources.Core.Domain.GroupResources
{
    public interface IGroupResource
    {
        string Name { get; }
        IReadOnlyCollection<GroupItem> Value { get; }
    }
}
