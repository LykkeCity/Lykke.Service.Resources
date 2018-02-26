using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.Resources.Core.Domain.TextResources
{
    public interface ITextResourceRepository
    {
        Task<IEnumerable<ITextResource>> GetAllAsync();
        Task<IEnumerable<ITextResource>> GetAllAsync(string lang, string name);
        Task<ITextResource> GetAsync(string lang, string name);
        Task<ITextResource> AddAsync(string lang, string name, string value);
        Task DeleteAsync(string lang, string name);
    }
}
