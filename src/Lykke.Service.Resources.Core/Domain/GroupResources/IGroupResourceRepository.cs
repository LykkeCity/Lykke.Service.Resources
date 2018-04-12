using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.Resources.Core.Domain.GroupResources
{
    public interface IGroupResourceRepository
    {
        Task<IEnumerable<IGroupResource>> GetAllAsync();
        Task<IEnumerable<IGroupResource>> GetAllAsync(string name);
        Task<IGroupResource> GetAsync(string name);
        Task<IGroupResource> AddAsync(IGroupResource resource);
        Task DeleteAsync(string name);
    }
}
