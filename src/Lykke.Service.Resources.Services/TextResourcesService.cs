using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.Resources.Core.Domain.TextResources;
using Lykke.Service.Resources.Core.Services;

namespace Lykke.Service.Resources.Services
{
    public class TextResourcesService : ITextResourcesService
    {
        private readonly ITextResourceRepository _repository;
        private ConcurrentDictionary<string, List<ITextResource>> _cache = new ConcurrentDictionary<string, List<ITextResource>>();

        public TextResourcesService(
            ITextResourceRepository repository
            )
        {
            _repository = repository;
        }
        
        public ITextResource Get(string lang, string name)
        {
            return _cache.TryGetValue(lang, out var items) 
                ? items.FirstOrDefault(item => item.Name == name) 
                : null;
        }

        public IEnumerable<ITextResource> GetAll(string lang, string name)
        {
            return _cache.TryGetValue(lang, out var items) 
                ? items.Where(item => item.Name.StartsWith(name)) 
                : Array.Empty<ITextResource>();
        }

        public IEnumerable<ITextResource> GetAll()
        {
            var result = new List<ITextResource>();

            foreach (var key in _cache.Keys)
            {
                if (_cache.TryGetValue(key, out var items))
                    result.AddRange(items);
            }
            
            return result.OrderBy(item => item.Lang).ThenBy(item => item.Name);
        }

        public async Task LoadAllAsync()
        {
            var resources = await _repository.GetAllAsync();
            var dict = resources.GroupBy(item => item.Lang).ToDictionary(item => item.Key, item => item.ToList());
            _cache = new ConcurrentDictionary<string, List<ITextResource>>(dict);
        }

        public async Task AddAsync(string lang, string name, string value)
        {
            var entity = await _repository.AddAsync(lang, name, value);

            if (!_cache.ContainsKey(lang))
            {
                _cache.TryAdd(lang, new List<ITextResource>());
            }

            if (_cache.TryGetValue(lang, out var items))
            {
                var resource = items.FirstOrDefault(item => item.Name == name);
                
                if (resource != null)
                    items.Remove(resource);

                items.Add(entity);
            }
        }

        public async Task DeleteAsync(string lang, string name)
        {
            await _repository.DeleteAsync(lang, name);
            
            if (_cache.TryGetValue(lang, out var items))
            {
                var resource = items.FirstOrDefault(item => item.Name == name);
                
                if (resource != null)
                    items.Remove(resource);
            }
        }
    }
}
