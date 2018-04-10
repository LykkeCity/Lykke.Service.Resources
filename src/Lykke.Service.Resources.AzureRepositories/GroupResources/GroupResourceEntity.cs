using System;
using System.Collections.Generic;
using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.ValueTypesMerging;
using Lykke.Service.Resources.Core.Domain.GroupResources;

namespace Lykke.Service.Resources.AzureRepositories.GroupResources
{
    [ValueTypeMergingStrategy(ValueTypeMergingStrategy.UpdateAlways)]
    public class GroupResourceEntity : AzureTableEntity, IGroupResource
    {
        public string Name { get; set; }
        [JsonValueSerializerAttribute]
        public IReadOnlyCollection<GroupItem> Value { get; set; }
        
        internal static string GeneratePartitionKey(string name)
        {
            string[] values = name.ToLower().Trim('.').Split('.', StringSplitOptions.RemoveEmptyEntries);
            
            return values.Length > 1 ? $"{string.Join(".", values, 0, values.Length - 1)}_group" : "group";
        }
        
        internal static string GenerateRowKey(string name)
        {
            string[] values = name.ToLower().Trim('.').Split('.', StringSplitOptions.RemoveEmptyEntries);
            
            return values.Length > 1 ? values[values.Length - 1] : values[0];
        }
        
        internal static GroupResourceEntity Create(IGroupResource resource)
        {
            return new GroupResourceEntity
            {
                PartitionKey = GeneratePartitionKey(resource.Name),
                RowKey = GenerateRowKey(resource.Name),
                Name = resource.Name,
                Value = resource.Value
            };
        }
    }
}
