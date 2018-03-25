using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.Resources.Models
{
    public class DeleteTextResourceModel
    {
        [Required]
        public string Lang { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
