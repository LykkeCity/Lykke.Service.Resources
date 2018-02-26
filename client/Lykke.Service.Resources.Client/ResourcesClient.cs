using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.Resources.Client.AutorestClient;
using Lykke.Service.Resources.Client.AutorestClient.Models;

namespace Lykke.Service.Resources.Client
{
    public class ResourcesClient : IResourcesClient, IDisposable
    {
        private readonly ILog _log;
        private ResourcesAPI _service;

        public ResourcesClient(string serviceUrl, ILog log)
        {
            _log = log;
            _service = new ResourcesAPI(new Uri(serviceUrl));
        }

        public void Dispose()
        {
            if (_service == null)
                return;
            _service.Dispose();
            _service = null;
        }

        public async Task<TextResource> GetTextResourceAsync(string lang, string name)
        {
            var response = await _service.GetTextResourceAsync(lang, name);

            if (response is TextResource result)
            {
                return result;
            }

            return null;
        }

        public async Task<IEnumerable<TextResource>> GetTextResourcesAsync(string lang, string name)
        {
            var response = await _service.GetTextResourcesAsync(lang, name);

            if (response is IEnumerable<TextResource> result)
            {
                return result;
            }

            return Array.Empty<TextResource>();
        }

        public async Task AddTextResourceAsync(string lang, string name, string value)
        {
            await _service.AddTextResourceAsync(new TextResourceModel(lang, name, value));
        }

        public async Task DeleteTextResourceAsync(string lang, string name)
        {
            await _service.DeleteTextResourceAsync(new DeleteTextResourceModel(lang, name));
        }

        public async Task<string> GetImageResourceAsync(string name)
        {
            var response = await _service.GetImageResourceAsync(name);

            if (response is string result)
            {
                return result;
            }

            return null;
        }

        public async Task AddImageResourceAsync(string name, byte[] data)
        {
            await _service.AddImageResourceAsync(new ImageResourceModel(name, data));
        }

        public async Task DeleteImageResourceAsync(string name)
        {
            await _service.DeleteImageResourceAsync(name);
        }
    }
}
