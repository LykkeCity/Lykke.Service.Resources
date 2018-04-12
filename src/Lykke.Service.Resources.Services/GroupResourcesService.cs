using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.Resources.Core.Domain.GroupResources;
using Lykke.Service.Resources.Core.Services;

namespace Lykke.Service.Resources.Services
{
    public class GroupResourcesService : IGroupResourcesService
    {
        private readonly IGroupResourceRepository _repository;
        private const string GroupDelimiter = ".";
        private Dictionary<string, IGroupResource> _cache = new Dictionary<string, IGroupResource>();

        public GroupResourcesService(
            IGroupResourceRepository repository
            )
        {
            _repository = repository;
        }
        
        public IGroupResource Get(string name)
        {
            return _cache.ContainsKey(name) ? _cache[name] : null;
        }

        public IEnumerable<IGroupResource> GetGroup(string groupName)
        {
            var prefix = string.IsNullOrEmpty(groupName) 
                ? string.Empty 
                : groupName.TrimEnd('.') + GroupDelimiter;

            return _cache.Where(item => item.Key.StartsWith(prefix)).Select(item => item.Value);
        }

        public IEnumerable<IGroupResource> GetAll()
        {
            return _cache.Values.OrderBy(item => item.Name);
        }

        public async Task LoadAllAsync()
        {
            var resources = await _repository.GetAllAsync();
            _cache = resources.Select(GroupResource.Create).ToDictionary(item => item.Name);
        }

        public async Task AddAsync(string name, GroupItem[] value)
        {
            var entity = await _repository.AddAsync(new GroupResource{Name = name, Value = value});

            if (_cache.ContainsKey(name))
                _cache.Remove(name);
            
            _cache.Add(name, GroupResource.Create(entity));
        }

        public async Task AddItemAsync(string name, GroupItem value)
        {
            var values = new List<GroupItem>();
            
            var group = Get(name);

            if (group != null)
            {
                values = group.Value.ToList();
                
                var groupItem = values.FirstOrDefault(item => item.Id == value.Id);
                
                if (groupItem != null)
                {
                    groupItem.Value = value.Value;
                }
                else
                {
                    values.Add(value);
                }
            }
            else
            {
                values.Add(value);
            }

            await AddAsync(name, values.ToArray());
        }

        public async Task DeleteAsync(string name)
        {
            await _repository.DeleteAsync(name);
            
            if (_cache.ContainsKey(name))
                _cache.Remove(name);
        }

        public async Task DeleteItemAsync(string name, string id)
        {
            var group = Get(name);

            if (group != null)
            {
                var values = group.Value.ToList();
                
                var groupItem = values.FirstOrDefault(item => item.Id == id);

                if (groupItem != null)
                {
                    values.Remove(groupItem);

                    if (values.Count == 0)
                    {
                        await DeleteAsync(name);
                    }
                    else
                    {
                        await AddAsync(name, values.ToArray());
                    }
                }
            }
        }
    }
}
