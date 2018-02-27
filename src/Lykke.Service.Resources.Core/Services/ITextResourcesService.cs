using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Resources.Core.Domain.TextResources;

namespace Lykke.Service.Resources.Core.Services
{
    public interface ITextResourcesService
    {
        ITextResource Get(string lang, string name);
        IEnumerable<ITextResource> GetAll(string lang, string name);
        IEnumerable<ITextResource> GetAll();
        Task LoadAllAsync();
        Task AddAsync(string lang, string name, string value);
        Task DeleteAsync(string lang, string name);
    }
}
