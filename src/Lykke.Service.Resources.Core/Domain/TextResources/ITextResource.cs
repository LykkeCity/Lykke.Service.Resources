namespace Lykke.Service.Resources.Core.Domain.TextResources
{
    public interface ITextResource
    {
        string Lang { get; }
        string Name { get; }
        string Value { get; }
    }
}
