namespace Structural.Flyweight.IntrinsicVsExtrinsicState.Api;

public sealed class GlyphFlyweight
{
    public GlyphFlyweight(char symbol, string fontFamily)
    {
        Symbol = symbol;
        FontFamily = fontFamily;
    }

    public char Symbol { get; }
    public string FontFamily { get; }

    public string Draw(int x, int y, string color) => $"{Symbol}@({x},{y}) font={FontFamily} color={color}";
}

public sealed class GlyphFactory
{
    private readonly Dictionary<string, GlyphFlyweight> _cache = new();

    public GlyphFlyweight Get(char symbol, string fontFamily)
    {
        var key = $"{symbol}:{fontFamily}";
        if (!_cache.TryGetValue(key, out var glyph))
        {
            glyph = new GlyphFlyweight(symbol, fontFamily);
            _cache[key] = glyph;
        }

        return glyph;
    }

    public int SharedInstances => _cache.Count;
}

public static class PatternDemo
{
    public static object Create()
    {
        var factory = new GlyphFactory();
        var renders = new List<string>();
        var word = "MOM";

        for (var i = 0; i < word.Length; i++)
        {
            var glyph = factory.Get(word[i], "Consolas");
            renders.Add(glyph.Draw(i * 10, 5, i % 2 == 0 ? "blue" : "black"));
        }

        return new
        {
            Pattern = "Flyweight",
            Variant = "Intrinsic vs Extrinsic State",
            Renders = renders,
            SharedFlyweights = factory.SharedInstances
        };
    }
}
