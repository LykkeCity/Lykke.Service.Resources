using System.ComponentModel.DataAnnotations;
using Lykke.Service.Resources.Core.Domain.GroupResources;

namespace Lykke.Service.Resources.Models
{
    public class GroupResourcesModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public GroupItem[] Values { get; set; }
    }
}
