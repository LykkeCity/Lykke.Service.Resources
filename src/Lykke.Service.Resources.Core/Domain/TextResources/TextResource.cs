namespace Lykke.Service.Resources.Core.Domain.TextResources
{
    public class TextResource : ITextResource
    {
        public string Lang { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public static ITextResource Create(ITextResource src)
        {
            return new TextResource
            {
                Lang = src.Lang,
                Name = src.Name,
                Value = src.Value
            };
        }
    }
}
