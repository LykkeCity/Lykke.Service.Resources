using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.Resources.Models
{
    public class ImageResourceModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public byte[] Data { get; set; }
    }
}
