using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.Resources.Client 
{
    public class ResourcesServiceClientSettings 
    {
        [HttpCheck("/api/isalive")]
        public string ServiceUrl {get; set;}
    }
}
