using Lykke.Service.Resources.Core.Domain.Languages;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Resources.AzureRepositories.Languages
{
    public class LanguageEntity : TableEntity, ILanguage
    {
        public string Code { get; set; }
        public string Name { get; set; }

        internal static string GeneratePartitionKey() => "ResourceLanguage";
        internal static string GenerateRowKey(string code) => code.ToLower();

        internal static LanguageEntity Create(string code, string name)
        {
            return new LanguageEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(code),
                Code = code,
                Name = name
            };
        }
    }
}
