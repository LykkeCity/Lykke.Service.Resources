using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.Resources.Core.Domain.GroupResources;

namespace Lykke.Service.Resources.AzureRepositories.GroupResources
{
    public class GroupResourcesRepository : IGroupResourceRepository
    {
        private readonly INoSQLTableStorage<GroupResourceEntity> _tableStorage;

        public GroupResourcesRepository(INoSQLTableStorage<GroupResourceEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }
        
        public async Task<IEnumerable<IGroupResource>> GetAllAsync()
        {
            var result = new List<IGroupResource>();

            await _tableStorage.GetDataByChunksAsync(entities => result.AddRange(entities));

            return result;
        }

        public async Task<IEnumerable<IGroupResource>> GetAllAsync(string name)
        {
            return await _tableStorage.GetDataAsync(GroupResourceEntity.GeneratePartitionKey(name));
        }

        public async Task<IGroupResource> GetAsync(string name)
        {
            return await _tableStorage.GetDataAsync(GroupResourceEntity.GeneratePartitionKey(name), GroupResourceEntity.GenerateRowKey(name));
        }

        public async Task<IGroupResource> AddAsync(IGroupResource resource)
        {
            var entity = GroupResourceEntity.Create(resource);
            await _tableStorage.InsertOrMergeAsync(entity);

            return entity;
        }

        public Task DeleteAsync(string name)
        {
            return _tableStorage.DeleteIfExistAsync(GroupResourceEntity.GeneratePartitionKey(name), GroupResourceEntity.GenerateRowKey(name));
        }
    }
}
