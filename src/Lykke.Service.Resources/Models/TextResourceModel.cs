using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.Resources.Models
{
    public class TextResourceModel
    {
        [Required]
        public string Lang { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
