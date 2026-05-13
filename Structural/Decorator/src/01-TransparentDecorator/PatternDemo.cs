namespace Structural.Decorator.TransparentDecorator.Api;

public interface IText
{
    string Render();
}

public sealed class PlainText : IText
{
    private readonly string _value;

    public PlainText(string value) => _value = value;

    public string Render() => _value;
}

public abstract class TextDecorator : IText
{
    protected readonly IText Inner;

    protected TextDecorator(IText inner) => Inner = inner;

    public abstract string Render();
}

public sealed class UppercaseDecorator : TextDecorator
{
    public UppercaseDecorator(IText inner) : base(inner) { }

    public override string Render() => Inner.Render().ToUpperInvariant();
}

public sealed class BracketDecorator : TextDecorator
{
    public BracketDecorator(IText inner) : base(inner) { }

    public override string Render() => $"[{Inner.Render()}]";
}

public static class PatternDemo
{
    public static object Create()
    {
        IText text = new PlainText("structural patterns");
        text = new UppercaseDecorator(text);
        text = new BracketDecorator(text);

        return new
        {
            Pattern = "Decorator",
            Variant = "Transparent Decorator",
            Rendered = text.Render()
        };
    }
}
