using Autofac;
using AzureStorage.Blob;
using AzureStorage.Tables;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Sdk;
using Lykke.Service.Resources.AzureRepositories.GroupResources;
using Lykke.Service.Resources.AzureRepositories.Languages;
using Lykke.Service.Resources.AzureRepositories.TextResources;
using Lykke.Service.Resources.Core.Domain.GroupResources;
using Lykke.Service.Resources.Core.Domain.Languages;
using Lykke.Service.Resources.Core.Domain.TextResources;
using Lykke.Service.Resources.Core.Services;
using Lykke.Service.Resources.Services;
using Lykke.Service.Resources.Settings;
using Lykke.SettingsReader;

namespace Lykke.Service.Resources.Modules
{
    [UsedImplicitly]
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<AppSettings> _settings;

        public ServiceModule(IReloadingManager<AppSettings> settings)
        {
            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StartupManager>()
                .As<IStartupManager>()
                .SingleInstance();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>()
                .SingleInstance();

            builder.Register(ctx =>
                new TextResourcesRepository(AzureTableStorage<TextResourceEntity>.Create(
                    _settings.ConnectionString(x => x.ResourcesService.Db.DataConnString), "TextResources", ctx.Resolve<ILogFactory>()))
            ).As<ITextResourceRepository>().SingleInstance();
            
            builder.Register(ctx =>
                new LanguagesRepository(AzureTableStorage<LanguageEntity>.Create(
                    _settings.ConnectionString(x => x.ResourcesService.Db.DataConnString), "ResourceLanguages", ctx.Resolve<ILogFactory>()))
            ).As<ILanguagesRepository>().SingleInstance();
            
            builder.Register(ctx =>
                new GroupResourcesRepository(AzureTableStorage<GroupResourceEntity>.Create(
                    _settings.ConnectionString(x => x.ResourcesService.Db.DataConnString), "GroupResources", ctx.Resolve<ILogFactory>()))
            ).As<IGroupResourceRepository>().SingleInstance();

            builder.RegisterType<TextResourcesService>()
                .As<ITextResourcesService>()
                .SingleInstance();

            builder.RegisterInstance<IImageResourcesService>(
                new ImageResourcesService(AzureBlobStorage.Create(_settings.ConnectionString(x => x.ResourcesService.Db.DataConnString)), 
                    _settings.CurrentValue.ResourcesService.ImagesContainer)
            ).SingleInstance();
            
            builder.RegisterType<LanguagesService>()
                .As<ILanguagesService>()
                .SingleInstance();
            
            builder.RegisterType<GroupResourcesService>()
                .As<IGroupResourcesService>()
                .SingleInstance();
        }
    }
}
