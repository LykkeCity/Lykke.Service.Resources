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
        private readonly ITextResourcesService _textResourcesService;
        private readonly List<ILanguage> _cache = new List<ILanguage>();

        public LanguagesService(
            ILanguagesRepository repository,
            ITextResourcesService textResourcesService
            )
        {
            _repository = repository;
            _textResourcesService = textResourcesService;
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
            _cache.AddRange(languages.Select(Language.Create));
        }

        public async Task AddAsync(string code, string name)
        {
            var language = await _repository.AddAsync(code, name);

            var existing = Get(code);
            
            if (existing != null)
                _cache.Remove(existing);
            
            _cache.Add(Language.Create(language));
        }

        public async Task DeleteAsync(string code)
        {
            await _repository.DeleteAsync(code);

            var language = Get(code);

            if (language != null)
                _cache.Remove(language);

            var resources = _textResourcesService.GetAll().Where(item => item.Lang == code);
            
            foreach (var resource in resources)
            {
                await _textResourcesService.DeleteAsync(resource.Lang, resource.Name);
            }
        }
    }
}
