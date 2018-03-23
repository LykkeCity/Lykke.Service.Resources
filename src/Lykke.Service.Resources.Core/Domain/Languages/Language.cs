namespace Lykke.Service.Resources.Core.Domain.Languages
{
    public class Language : ILanguage
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public static ILanguage Create(ILanguage src)
        {
            return new Language
            {
                Code = src.Code,
                Name = src.Name
            };
        }
    }
}
