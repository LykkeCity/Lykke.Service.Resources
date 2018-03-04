namespace Lykke.Service.Resources.Settings.ServiceSettings
{
    public class ResourcesSettings
    {
        public int MaxFileSizeInMb { get; set; }
        public string ImagesContainer { get; set; }
        public DbSettings Db { get; set; }
    }
}
