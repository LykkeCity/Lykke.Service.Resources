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
            return await _service.GetTextResourceAsync(lang, name);
        }

        public async Task<IEnumerable<TextResource>> GetTextResourcesAsync(string lang, string name)
        {
            return await _service.GetTextResourcesAsync(lang, name);
        }

        public async Task<IEnumerable<TextResource>> GetAllTextResourcesAsync()
        {
            return await _service.GetAllTextResourcesAsync();
        }

        public async Task AddTextResourceAsync(string lang, string name, string value)
        {
            var response = await _service.AddTextResourceAsync(new TextResourceModel(lang, name, value));
            
            if (response != null)
                throw new Exception(response.ErrorMessage);
        }

        public async Task DeleteTextResourceAsync(string lang, string name)
        {
            var response = await _service.DeleteTextResourceAsync(new DeleteTextResourceModel(lang, name));
            
            if (response != null)
                throw new Exception(response.ErrorMessage);
        }

        public async Task<string> GetImageResourceAsync(string name)
        {
            return await _service.GetImageResourceAsync(name);
        }

        public async Task<IEnumerable<ImageResource>> GetAllImageResourcesAsync()
        {
            return await _service.GetAllImageResourcesAsync();
        }

        public async Task AddImageResourceAsync(string name, byte[] data)
        {
            var response = await _service.AddImageResourceAsync(new ImageResourceModel(name, data));
            
            if (response != null)
                throw new Exception(response.ErrorMessage);
        }

        public async Task DeleteImageResourceAsync(string name)
        {
            var response = await _service.DeleteImageResourceAsync(name);
            
            if (response != null)
                throw new Exception(response.ErrorMessage);
        }

        public async Task<IEnumerable<Language>> GetAllLanguagesAsync()
        {
            return await _service.GetAllLanguagesAsync();
        }

        public async Task AddLanguageAsync(string code, string name)
        {
            var response = await _service.AddLanguageAsync(new Language(code, name));
            
            if (response != null)
                throw new Exception(response.ErrorMessage);
        }

        public async Task DeleteLanguageAsync(string code)
        {
            var response = await _service.DeleteLanguageAsync(code);
            
            if (response != null)
                throw new Exception(response.ErrorMessage);
        }
    }
}
