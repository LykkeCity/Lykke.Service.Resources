using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.Resources.Core.Domain.Languages;
using Lykke.Service.Resources.Core.Services;

namespace Lykke.Service.Resources.Services
{
    public class LanguagesService : ILanguagesService
    {
        private readonly ILanguagesRepository _repository;
        private readonly List<ILanguage> _cache = new List<ILanguage>();

        public LanguagesService(
            ILanguagesRepository repository
            )
        {
            _repository = repository;
        }
        
        public ILanguage Get(string code)
        {
            return _cache.FirstOrDefault(item => item.Code == code);
        }

        public IEnumerable<ILanguage> GetAll()
        {
            return _cache.OrderBy(item => item.Code);
        }

        public async Task LoadAllAsync()
        {
            var languages = await _repository.GetAllAsync();
            _cache.AddRange(languages);
        }

        public async Task AddAsync(string code, string name)
        {
            var language = await _repository.AddAsync(code, name);
            _cache.Add(language);
        }

        public async Task DeleteAsync(string code)
        {
            await _repository.DeleteAsync(code);

            var language = Get(code);

            if (language != null)
                _cache.Remove(language);
        }
    }
}
