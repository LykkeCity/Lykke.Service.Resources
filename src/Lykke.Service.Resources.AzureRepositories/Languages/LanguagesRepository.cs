using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.Resources.Core.Domain.Languages;

namespace Lykke.Service.Resources.AzureRepositories.Languages
{
    public class LanguagesRepository : ILanguagesRepository
    {
        private readonly INoSQLTableStorage<LanguageEntity> _tableStorage;

        public LanguagesRepository(INoSQLTableStorage<LanguageEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }
        
        public async Task<IEnumerable<ILanguage>> GetAllAsync()
        {
            return await _tableStorage.GetDataAsync(LanguageEntity.GeneratePartitionKey());
        }

        public async Task<ILanguage> AddAsync(string code, string name)
        {
            var entity = LanguageEntity.Create(code, name);
            await _tableStorage.InsertOrMergeAsync(entity);
            return entity;
        }

        public async Task DeleteAsync(string code)
        {
            await _tableStorage.DeleteIfExistAsync(LanguageEntity.GeneratePartitionKey(), LanguageEntity.GenerateRowKey(code));
        }
    }
}
