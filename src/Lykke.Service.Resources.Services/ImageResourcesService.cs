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
        private readonly string _imageResourceContainer;
        private readonly List<ImageResource> _cache = new List<ImageResource>();

        public ImageResourcesService(
            IBlobStorage blobStorage,
            string imageResourceContainer
            )
        {
            _blobStorage = blobStorage;
            _imageResourceContainer = imageResourceContainer;
        }
        
        public string Get(string name)
        {
            return _cache.FirstOrDefault(item => item.Name == name)?.Url;
        }

        public IEnumerable<ImageResource> GetAll()
        {
            return _cache.OrderBy(item => item.Name);
        }

        public async Task LoadAllAsync()
        {
            var names = await _blobStorage.GetListOfBlobKeysAsync(_imageResourceContainer);
            
            foreach (var name in names)
            {
                _cache.Add(new ImageResource{Name = name, Url = _blobStorage.GetBlobUrl(_imageResourceContainer, name)});
            }
        }

        public async Task AddAsync(string name, byte[] data)
        {
            if (!await _blobStorage.HasBlobAsync(_imageResourceContainer, name))
            {
                await _blobStorage.SaveBlobAsync(_imageResourceContainer, name, data);
                _cache.Add(new ImageResource{Name = name, Url = _blobStorage.GetBlobUrl(_imageResourceContainer, name)});
            }
        }

        public async Task<bool> IsFileExistsAsync(string name)
        {
            return await _blobStorage.HasBlobAsync(_imageResourceContainer, name);
        }

        public async Task DeleteAsync(string name)
        {
            if (await _blobStorage.HasBlobAsync(_imageResourceContainer, name))
                await _blobStorage.DelBlobAsync(_imageResourceContainer, name);
            
            var image = _cache.FirstOrDefault(item => item.Name == name);

            if (image != null)
                _cache.Remove(image);
        }
    }
}
