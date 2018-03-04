using Autofac;
using AzureStorage.Blob;
using AzureStorage.Tables;
using Common.Log;
using Lykke.Service.Resources.AzureRepositories.Languages;
using Lykke.Service.Resources.AzureRepositories.TextResources;
using Lykke.Service.Resources.Core.Domain.Languages;
using Lykke.Service.Resources.Core.Domain.TextResources;
using Lykke.Service.Resources.Core.Services;
using Lykke.Service.Resources.Settings.ServiceSettings;
using Lykke.Service.Resources.Services;
using Lykke.SettingsReader;

namespace Lykke.Service.Resources.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<ResourcesSettings> _settings;
        private readonly ILog _log;

        public ServiceModule(IReloadingManager<ResourcesSettings> settings, ILog log)
        {
            _settings = settings;
            _log = log;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();
            
            builder.RegisterInstance(_settings.CurrentValue)
                .SingleInstance();

            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>()
                .SingleInstance();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>()
                .SingleInstance();

            builder.RegisterInstance<ITextResourceRepository>(
                new TextResourcesRepository(AzureTableStorage<TextResourceEntity>.Create(
                    _settings.ConnectionString(x => x.Db.DataConnString), "TextResources", _log))
            ).SingleInstance();
            
            builder.RegisterInstance<ILanguagesRepository>(
                new LanguagesRepository(AzureTableStorage<LanguageEntity>.Create(
                    _settings.ConnectionString(x => x.Db.DataConnString), "ResourceLanguages", _log))
            ).SingleInstance();

            builder.RegisterType<TextResourcesService>()
                .As<ITextResourcesService>()
                .SingleInstance();

            builder.RegisterInstance<IImageResourcesService>(
                new ImageResourcesService(AzureBlobStorage.Create(_settings.ConnectionString(x => x.Db.DataConnString)), _settings.CurrentValue.ImagesContainer)
            ).SingleInstance();
        }
    }
}
