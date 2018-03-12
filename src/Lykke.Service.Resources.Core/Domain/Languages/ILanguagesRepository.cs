using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.Resources.Core.Domain.Languages
{
    public interface ILanguagesRepository
    {
        Task<IEnumerable<ILanguage>> GetAllAsync();
        Task<ILanguage> AddAsync(string code, string name);
        Task DeleteAsync(string code);
    }
}
