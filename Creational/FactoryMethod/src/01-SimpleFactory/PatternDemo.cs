namespace FactoryMethod.SimpleFactory.Api;

public interface IRenderer
{
    string Kind { get; }
    string Render(string value);
}

public sealed class PdfRenderer : IRenderer
{
    public string Kind => "PDF";
    public string Render(string value) => $"[PDF] {value}";
}

public sealed class TextRenderer : IRenderer
{
    public string Kind => "Text";
    public string Render(string value) => $"[TEXT] {value}";
}

public static class StaticRendererFactory
{
    public static IRenderer Create(string output)
        => output.ToLowerInvariant() switch
        {
            "pdf" => new PdfRenderer(),
            _ => new TextRenderer()
        };
}

public static class SimpleFactoryDemo
{
    public static object Create()
    {
        var renderer = StaticRendererFactory.Create("pdf");
        return new
        {
            Pattern = "Factory Method",
            Variant = "Simple Factory (static)",
            Product = renderer.Kind,
            Output = renderer.Render("Quarterly report")
        };
    }
}
