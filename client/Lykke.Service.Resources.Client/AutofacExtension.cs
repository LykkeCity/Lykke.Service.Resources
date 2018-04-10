using System;
using Autofac;

namespace Lykke.Service.Resources.Client
{
    public static class AutofacExtension
    {
        public static void RegisterResourcesClient(this ContainerBuilder builder, string serviceUrl)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (serviceUrl == null) throw new ArgumentNullException(nameof(serviceUrl));
            if (string.IsNullOrWhiteSpace(serviceUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceUrl));

            builder.RegisterType<ResourcesClient>()
                .WithParameter("serviceUrl", serviceUrl)
                .As<IResourcesClient>()
                .SingleInstance();
        }

        public static void RegisterResourcesClient(this ContainerBuilder builder, ResourcesServiceClientSettings settings)
        {
            builder.RegisterResourcesClient(settings?.ServiceUrl);
        }
    }
}
