using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.Resources.Core.Domain.ImageResources;
using Lykke.Service.Resources.Core.Services;

namespace Lykke.Service.Resources.Services
{
    public class ImageResourcesService : IImageResourcesService
    {
        private readonly IBlobStorage _blobStorage;
        private const string ImageResourcesContainer = "imageresources";
        private readonly List<IImageResource> _cache = new List<IImageResource>();

        public ImageResourcesService(IBlobStorage blobStorage)
        {
            _blobStorage = blobStorage;
        }
        
        public string Get(string name)
        {
            return _cache.FirstOrDefault(item => item.Name == name)?.Url;
        }

        public async Task LoadAllAsync()
        {
            var names = await _blobStorage.GetListOfBlobKeysAsync(ImageResourcesContainer);
            
            foreach (var name in names)
            {
                _cache.Add(new ImageResource{Name = name, Url = _blobStorage.GetBlobUrl(ImageResourcesContainer, name)});
            }
        }

        public async Task AddAsync(string name, byte[] data)
        {
            await _blobStorage.SaveBlobAsync(ImageResourcesContainer, name, data);
            _cache.Add(new ImageResource{Name = name, Url = _blobStorage.GetBlobUrl(ImageResourcesContainer, name)});
        }

        public async Task DeleteAsync(string name)
        {
            if (await _blobStorage.HasBlobAsync(ImageResourcesContainer, name))
                await _blobStorage.DelBlobAsync(ImageResourcesContainer, name);
            
            var image = _cache.FirstOrDefault(item => item.Name == name);

            if (image != null)
                _cache.Remove(image);
        }
    }
}
