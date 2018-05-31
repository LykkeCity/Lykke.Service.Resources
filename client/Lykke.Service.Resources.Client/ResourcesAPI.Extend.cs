using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

// ReSharper disable once CheckNamespace
namespace Lykke.Service.Resources.Client.AutorestClient
{

    public partial class ResourcesAPI
    {
        /// <summary>
        /// Should be used to prevent memory leak in RetryPolicy
        /// </summary>
        public ResourcesAPI(Uri baseUri, HttpClient client) : base(client)
        {
            Initialize();
            BaseUri = baseUri ?? throw new ArgumentNullException("baseUri");
        }
    }
}
