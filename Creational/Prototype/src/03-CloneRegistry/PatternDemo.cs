namespace Prototype.CloneRegistry.Api;

public sealed class TemplateDocument
{
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;

    public TemplateDocument Clone()
        => new()
        {
            Title = Title,
            Category = Category
        };
}

public sealed class DocumentRegistry
{
    private readonly Dictionary<string, TemplateDocument> _templates = new(StringComparer.OrdinalIgnoreCase);

    public void Register(string key, TemplateDocument template) => _templates[key] = template;

    public TemplateDocument Create(string key) => _templates[key].Clone();
}

public static class CloneRegistryDemo
{
    public static object Create()
    {
        var registry = new DocumentRegistry();
        registry.Register("invoice", new TemplateDocument { Title = "Invoice Template", Category = "Billing" });

        var clone = registry.Create("invoice");
        clone.Title = "Invoice for April";

        return new
        {
            Pattern = "Prototype",
            Variant = "Clone Registry",
            clone.Title,
            clone.Category
        };
    }
}
