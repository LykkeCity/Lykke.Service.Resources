using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Sdk;
using Lykke.Service.Resources.Core.Services;

namespace Lykke.Service.Resources.Services
{
    // NOTE: Sometimes, startup process which is expressed explicitly is not just better, 
    // but the only way. If this is your case, use this class to manage startup.
    // For example, sometimes some state should be restored before any periodical handler will be started, 
    // or any incoming message will be processed and so on.
    // Do not forget to remove As<IStartable>() and AutoActivate() from DI registartions of services, 
    // which you want to startup explicitly.

    public class StartupManager : IStartupManager
    {
        private readonly ITextResourcesService _textResourcesService;
        private readonly IImageResourcesService _imageResourcesService;
        private readonly ILanguagesService _languagesService;
        private readonly IGroupResourcesService _groupResourcesService;
        private readonly ILog _log;

        public StartupManager(
            ITextResourcesService textResourcesService,
            IImageResourcesService imageResourcesService,
            ILanguagesService languagesService,
            IGroupResourcesService groupResourcesService,
            ILogFactory logFactory)
        {
            _textResourcesService = textResourcesService;
            _imageResourcesService = imageResourcesService;
            _languagesService = languagesService;
            _groupResourcesService = groupResourcesService;
            _log = logFactory.CreateLog(this);
        }

        public async Task StartAsync()
        {
            _log.Info(nameof(StartAsync), "Filling text resources cache...");
            await _textResourcesService.LoadAllAsync();
            _log.Info(nameof(StartAsync), "Text resources cache is initialized");
            
            _log.Info(nameof(StartAsync), "Filling image resources cache...");
            await _imageResourcesService.LoadAllAsync();
            _log.Info(nameof(StartAsync), "Image resources cache is initialized");
            
            _log.Info(nameof(StartAsync), "Filling group resources cache...");
            await _groupResourcesService.LoadAllAsync();
            _log.Info(nameof(StartAsync), "Group resources cache is initialized");
            
            _log.Info(nameof(StartAsync), "Filling languages cache...");
            await _languagesService.LoadAllAsync();
            _log.Info(nameof(StartAsync), "Languages cache is initialized");
        }
    }
}
