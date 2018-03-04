using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.Resources.Models
{
    public class Language
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
