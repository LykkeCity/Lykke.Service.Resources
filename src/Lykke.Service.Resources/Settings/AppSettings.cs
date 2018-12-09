using JetBrains.Annotations;
using Lykke.Sdk.Settings;
using Lykke.Service.Resources.Settings.ServiceSettings;

namespace Lykke.Service.Resources.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public ResourcesSettings ResourcesService { get; set; }
    }
}
