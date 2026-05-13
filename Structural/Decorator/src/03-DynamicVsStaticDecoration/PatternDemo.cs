namespace Structural.Decorator.DynamicVsStaticDecoration.Api;

public interface IContent
{
    string Value();
}

public sealed class BaseContent : IContent
{
    private readonly string _value;

    public BaseContent(string value) => _value = value;

    public string Value() => _value;
}

public sealed class UpperDecorator : IContent
{
    private readonly IContent _inner;

    public UpperDecorator(IContent inner) => _inner = inner;

    public string Value() => _inner.Value().ToUpperInvariant();
}

public sealed class StarDecorator : IContent
{
    private readonly IContent _inner;

    public StarDecorator(IContent inner) => _inner = inner;

    public string Value() => $"*{_inner.Value()}*";
}

public static class PatternDemo
{
    public static object Create()
    {
        var staticDecorated = new StarDecorator(new UpperDecorator(new BaseContent("compile time")));

        IContent dynamicDecorated = new BaseContent("runtime");
        var decorators = new Func<IContent, IContent>[]
        {
            c => new UpperDecorator(c),
            c => new StarDecorator(c)
        };

        foreach (var decorate in decorators)
        {
            dynamicDecorated = decorate(dynamicDecorated);
        }

        return new
        {
            Pattern = "Decorator",
            Variant = "Dynamic vs Static Decoration",
            StaticDecoration = staticDecorated.Value(),
            DynamicDecoration = dynamicDecorated.Value()
        };
    }
}
