using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Resources.Core.Domain.ImageResources;

namespace Lykke.Service.Resources.Core.Services
{
    public interface IImageResourcesService
    {
        string Get(string name);
        IEnumerable<ImageResource> GetAll();
        Task LoadAllAsync();
        Task AddAsync(string name, byte[] data);
        Task DeleteAsync(string name);
    }
}
