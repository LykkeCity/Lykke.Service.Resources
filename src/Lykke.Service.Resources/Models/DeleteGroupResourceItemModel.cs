using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.Resources.Models
{
    public class DeleteGroupResourceItemModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Id { get; set; }
    }
}
