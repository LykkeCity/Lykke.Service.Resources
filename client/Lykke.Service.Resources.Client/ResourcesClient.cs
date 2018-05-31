using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Service.Resources.Client.AutorestClient;
using Lykke.Service.Resources.Client.AutorestClient.Models;

namespace Lykke.Service.Resources.Client
{
    /// <inheritdoc cref="IResourcesClient"/>>
    public class ResourcesClient : IResourcesClient, IDisposable
    {
        private ResourcesAPI _service;

        /// <summary>
        /// </summary>
        /// <param name="serviceUrl"></param>
        /// <param name="log"></param>
        [UsedImplicitly]
        public ResourcesClient(string serviceUrl)
        {
            _service = new ResourcesAPI(new Uri(serviceUrl), new HttpClient());
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_service == null)
                return;
            _service.Dispose();
            _service = null;
        }

        /// <inheritdoc />
        public async Task<TextResource> GetTextResourceAsync(string lang, string name)
        {
            var response = await _service.GetTextResourceAsync(lang, name);

            switch (response)
            {
                case TextResource result:
                    return result;
                case ErrorResponse error:
                    throw new Exception(error.ErrorMessage);
            }

            return null;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TextResource>> GetTextResourceSectionAsync(string lang, string name)
        {
            var response = await _service.GetTextResourceSectionAsync(lang, name);

            switch (response)
            {
                case List<TextResource> result:
                    return result;
                case ErrorResponse error:
                    throw new Exception(error.ErrorMessage);
            }

            return Array.Empty<TextResource>();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TextResource>> GetAllTextResourcesAsync()
        {
            return await _service.GetAllTextResourcesAsync();
        }

        /// <inheritdoc />
        public async Task AddTextResourceAsync(string lang, string name, string value)
        {
            var response = await _service.AddTextResourceAsync(new TextResourceModel(lang, name, value));

            if (response != null)
                throw new Exception(response.ErrorMessage);
        }

        /// <inheritdoc />
        public async Task DeleteTextResourceAsync(string lang, string name)
        {
            var response = await _service.DeleteTextResourceAsync(new DeleteTextResourceModel(lang, name));

            if (response != null)
                throw new Exception(response.ErrorMessage);
        }

        /// <inheritdoc />
        public async Task<string> GetImageResourceAsync(string name)
        {
            return await _service.GetImageResourceAsync(name);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ImageResource>> GetAllImageResourcesAsync()
        {
            return await _service.GetAllImageResourcesAsync();
        }

        /// <inheritdoc />
        public async Task AddImageResourceAsync(string name, byte[] data)
        {
            var response = await _service.AddImageResourceAsync(new ImageResourceModel(name, data));

            if (response != null)
                throw new Exception(response.ErrorMessage);
        }

        /// <inheritdoc />
        public async Task DeleteImageResourceAsync(string name)
        {
            var response = await _service.DeleteImageResourceAsync(name);

            if (response != null)
                throw new Exception(response.ErrorMessage);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Language>> GetAllLanguagesAsync()
        {
            return await _service.GetAllLanguagesAsync();
        }

        /// <inheritdoc />
        public async Task AddLanguageAsync(string code, string name)
        {
            var response = await _service.AddLanguageAsync(new Language(code, name));

            if (response != null)
                throw new Exception(response.ErrorMessage);
        }

        /// <inheritdoc />
        public async Task DeleteLanguageAsync(string code)
        {
            var response = await _service.DeleteLanguageAsync(code);

            if (response != null)
                throw new Exception(response.ErrorMessage);
        }

        /// <inheritdoc />
        public async Task<GroupResource> GetGroupResourceAsync(string name)
        {
            var response = await _service.GetGroupResourceAsync(name);

            switch (response)
            {
                case GroupResource result:
                    return result;
                case ErrorResponse error:
                    throw new Exception(error.ErrorMessage);
            }

            return null;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<GroupResource>> GetGroupResourceSectionAsync(string name)
        {
            var response = await _service.GetGroupResourceSectionAsync(name);

            switch (response)
            {
                case List<GroupResource> result:
                    return result;
                case ErrorResponse error:
                    throw new Exception(error.ErrorMessage);
            }

            return Array.Empty<GroupResource>();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<GroupResource>> GetAllGroupResourcesAsync()
        {
            return await _service.GetAllGroupResourcesAsync();
        }

        /// <inheritdoc />
        public async Task AddGroupResourcesAsync(string name, GroupItem[] values)
        {
            var response = await _service.AddGroupResourcesAsync(new GroupResourcesModel { Name = name, Values = values });

            if (response != null)
                throw new Exception(response.ErrorMessage);
        }

        /// <inheritdoc />
        public async Task AddGroupResourceItemAsync(string name, GroupItem value)
        {
            var response = await _service.AddGroupResourceItemAsync(new GroupResourceModel { Name = name, Value = value });

            if (response != null)
                throw new Exception(response.ErrorMessage);
        }

        /// <inheritdoc />
        public async Task DeleteGroupResourceAsync(string name)
        {
            var response = await _service.DeleteGroupResourceAsync(new DeleteGroupResourceModel { Name = name });

            if (response != null)
                throw new Exception(response.ErrorMessage);
        }

        /// <inheritdoc />
        public async Task DeleteGroupResourceItemAsync(string name, string id)
        {
            var response = await _service.DeleteGroupResourceItemAsync(new DeleteGroupResourceItemModel { Name = name, Id = id });

            if (response != null)
                throw new Exception(response.ErrorMessage);
        }
    }
}
