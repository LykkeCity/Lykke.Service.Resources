using System.Threading.Tasks;
using Common.Log;
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
        private readonly ILog _log;

        public StartupManager(
            ITextResourcesService textResourcesService,
            IImageResourcesService imageResourcesService,
            ILanguagesService languagesService,
            ILog log)
        {
            _textResourcesService = textResourcesService;
            _imageResourcesService = imageResourcesService;
            _languagesService = languagesService;
            _log = log;
        }

        public async Task StartAsync()
        {
            _log.WriteInfo(nameof(StartAsync), null, "Filling text resources cache...");
            await _textResourcesService.LoadAllAsync();
            _log.WriteInfo(nameof(StartAsync), null, "Text resources cache is initialized");
            
            _log.WriteInfo(nameof(StartAsync), null, "Filling image resources cache...");
            await _imageResourcesService.LoadAllAsync();
            _log.WriteInfo(nameof(StartAsync), null, "Image resources cache is initialized");
            
            _log.WriteInfo(nameof(StartAsync), null, "Filling languages cache...");
            await _languagesService.LoadAllAsync();
            _log.WriteInfo(nameof(StartAsync), null, "Languages cache is initialized");
        }
    }
}
