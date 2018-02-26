using System.Threading.Tasks;

namespace Lykke.Service.Resources.Core.Services
{
    public interface IImageResourcesService
    {
        string Get(string name);
        Task LoadAllAsync();
        Task AddAsync(string name, byte[] data);
        Task DeleteAsync(string name);
    }
}
