namespace Lykke.Service.Resources.Core.Domain.ImageResources
{
    public class ImageResource : IImageResource
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public static IImageResource Create(IImageResource src)
        {
            return new ImageResource
            {
                Name = src.Name,
                Url = src.Url
            };
        }
    }
}
