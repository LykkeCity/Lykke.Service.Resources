using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Resources.Core.Domain.Languages;

namespace Lykke.Service.Resources.Core.Services
{
    public interface ILanguagesService
    {
        ILanguage Get(string code);
        IEnumerable<ILanguage> GetAll();
        Task LoadAllAsync();
        Task AddAsync(string code, string name);
        Task DeleteAsync(string code);
    }
}
