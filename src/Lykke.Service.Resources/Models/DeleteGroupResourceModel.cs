using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.Resources.Models
{
    public class DeleteGroupResourceModel
    {
        [Required]
        public string Name { get; set; }
    }
}
