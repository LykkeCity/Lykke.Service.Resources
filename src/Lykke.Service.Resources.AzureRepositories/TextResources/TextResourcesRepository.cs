using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.Resources.Core.Domain.TextResources;

namespace Lykke.Service.Resources.AzureRepositories.TextResources
{
    public class TextResourcesRepository : ITextResourceRepository
    {
        private readonly INoSQLTableStorage<TextResourceEntity> _tableStorage;

        public TextResourcesRepository(INoSQLTableStorage<TextResourceEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IEnumerable<ITextResource>> GetAllAsync()
        {
            var result = new List<ITextResource>();

            await _tableStorage.GetDataByChunksAsync(entities => result.AddRange(entities));

            return result;
        }

        public async Task<IEnumerable<ITextResource>> GetAllAsync(string lang, string name)
        {
            return await _tableStorage.GetDataAsync(TextResourceEntity.GeneratePartitionKey(lang, name));
        }

        public async Task<ITextResource> GetAsync(string lang, string name)
        {
            return await _tableStorage.GetDataAsync(TextResourceEntity.GeneratePartitionKey(lang, name), TextResourceEntity.GenerateRowKey(name));
        }

        public async Task<ITextResource> AddAsync(string lang, string name, string value)
        {
            var entity = TextResourceEntity.Create(lang, name, value);
            await _tableStorage.InsertOrMergeAsync(entity);

            return entity;
        }

        public async Task DeleteAsync(string lang, string name)
        {
            await _tableStorage.DeleteIfExistAsync(TextResourceEntity.GeneratePartitionKey(lang, name), TextResourceEntity.GenerateRowKey(name));
        }
    }
}
