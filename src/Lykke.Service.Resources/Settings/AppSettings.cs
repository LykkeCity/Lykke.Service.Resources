using Lykke.Service.Resources.Settings.ServiceSettings;
using Lykke.Service.Resources.Settings.SlackNotifications;

namespace Lykke.Service.Resources.Settings
{
    public class AppSettings
    {
        public ResourcesSettings ResourcesService { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
    }
}
