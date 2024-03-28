using Microsoft.Extensions.Localization;

namespace TestProject1.Localization.Quantity;

internal class StringLocalizerStub : IStringLocalizer
{
    public Dictionary<string, string> Strings { get; } = new();

    public LocalizedString this[string name] => new(name, Strings[name]);

    public LocalizedString this[string name, params object[] arguments] => throw new NotImplementedException();

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        foreach (var (key, value) in Strings)
        {
            yield return new LocalizedString(key, value);
        }
    }
}