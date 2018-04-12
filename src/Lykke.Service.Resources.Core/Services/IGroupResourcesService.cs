using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Resources.Core.Domain.GroupResources;

namespace Lykke.Service.Resources.Core.Services
{
    public interface IGroupResourcesService
    {
        IGroupResource Get(string name);
        IEnumerable<IGroupResource> GetGroup(string groupName);
        IEnumerable<IGroupResource> GetAll();
        Task LoadAllAsync();
        Task AddAsync(string name, GroupItem[] values);
        Task AddItemAsync(string name, GroupItem value);
        Task DeleteAsync(string name);
        Task DeleteItemAsync(string name, string id);
    }
}
