using System;
using Lykke.Service.Resources.Core.Domain.TextResources;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Resources.AzureRepositories.TextResources
{
    public class TextResourceEntity : TableEntity, ITextResource
    {
        public string Lang { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        
        internal static string GeneratePartitionKey(string lang, string name)
        {
            string[] values = name.ToLower().Trim('.').Split('.', StringSplitOptions.RemoveEmptyEntries);
            
            return values.Length > 1 ? $"{string.Join(".", values, 0, values.Length - 1)}_{lang}" : $"_{lang}";
        }
        
        internal static string GenerateRowKey(string name)
        {
            string[] values = name.ToLower().Trim('.').Split('.', StringSplitOptions.RemoveEmptyEntries);
            
            return values.Length > 1 ? values[values.Length - 1] : values[0];
        }

        public static TextResourceEntity Create(string lang, string name, string value)
        {
            return new TextResourceEntity
            {
                PartitionKey = GeneratePartitionKey(lang, name),
                RowKey = GenerateRowKey(name),
                Lang = lang,
                Name = name,
                Value = value
            };
        }
    }
}
