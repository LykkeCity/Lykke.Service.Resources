﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Resources.Client.AutorestClient.Models;

namespace Lykke.Service.Resources.Client
{
    /// <summary>
    /// Client for text and image resources
    /// </summary>
    public interface IResourcesClient
    {
        /// <summary>
        /// Gets text resource by language and name
        /// </summary>
        /// <param name="lang">language</param>
        /// <param name="name">name of the resource</param>
        /// <returns></returns>
        Task<TextResource> GetTextResourceAsync(string lang, string name);

        /// <summary>
        /// Gets section of text resources
        /// </summary>
        /// <remarks>Returns text resources in the specified section, for example: lykke.ios will return all text resources under this section (lykke.ios.text, lykke.ios.title etc.)</remarks>
        /// <param name="lang">language</param>
        /// <param name="name">full name of the section</param>
        /// <returns></returns>
        Task<IEnumerable<TextResource>> GetTextResourceSectionAsync(string lang, string name);
        
        /// <summary>
        /// Gets list of all text resources
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TextResource>> GetAllTextResourcesAsync();
        
        /// <summary>
        /// Adds text resource
        /// </summary>
        /// <param name="lang">language</param>
        /// <param name="name">name of the resource</param>
        /// <param name="value">value of the resource</param>
        /// <returns></returns>
        Task AddTextResourceAsync(string lang, string name, string value);

        /// <summary>
        /// Deletes text resource by language and resource name
        /// </summary>
        /// <param name="lang">language</param>
        /// <param name="name">name of the resource</param>
        /// <returns></returns>
        Task DeleteTextResourceAsync(string lang, string name);
        
        /// <summary>
        /// Gets image resource (link of the blob)
        /// </summary>
        /// <param name="name">name of the resource(filename)</param>
        /// <returns></returns>
        Task<string> GetImageResourceAsync(string name);
        
        /// <summary>
        /// Gets list of all image resources
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ImageResource>> GetAllImageResourcesAsync();
        
        /// <summary>
        /// Adds image resource
        /// </summary>
        /// <param name="name">name of the resource(filename)</param>
        /// <param name="data">file content</param>
        /// <returns></returns>
        Task AddImageResourceAsync(string name, byte[] data);
        
        /// <summary>
        /// Deletes image resource by the name
        /// </summary>
        /// <param name="name">name of the resource(filename)</param>
        /// <returns></returns>
        Task DeleteImageResourceAsync(string name);
        
        
        /// <summary>
        /// Gets list of all languages
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Language>> GetAllLanguagesAsync();
        
        /// <summary>
        /// Adds a language
        /// </summary>
        /// <param name="code">code of the language (i.e. en)</param>
        /// <param name="name">name of the language (i.e. English)</param>
        /// <returns></returns>
        Task AddLanguageAsync(string code, string name);
        
        /// <summary>
        /// Deletes language by the code
        /// </summary>
        /// <param name="code">code of the language (i.e en)</param>
        /// <returns></returns>
        Task DeleteLanguageAsync(string code);
    }
}
