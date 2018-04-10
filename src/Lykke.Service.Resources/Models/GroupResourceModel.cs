using System.ComponentModel.DataAnnotations;
using Lykke.Service.Resources.Core.Domain.GroupResources;

namespace Lykke.Service.Resources.Models
{
    public class GroupResourceModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public GroupItem Value { get; set; }
    }
}
