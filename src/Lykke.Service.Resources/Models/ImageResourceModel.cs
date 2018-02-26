using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.Resources.Models
{
    public class ImageResourceModel
    {
        [Required]
        public string Name { get; set; }
        public byte[] Data { get; set; }
    }
}
