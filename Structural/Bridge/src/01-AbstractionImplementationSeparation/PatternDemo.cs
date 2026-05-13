namespace Structural.Bridge.AbstractionImplementationSeparation.Api;

public interface IRenderer
{
    string RenderCircle(float radius);
}

public sealed class VectorRenderer : IRenderer
{
    public string RenderCircle(float radius) => $"Vector circle radius={radius:0.0}";
}

public sealed class RasterRenderer : IRenderer
{
    public string RenderCircle(float radius) => $"Raster circle radius={radius:0.0}";
}

public abstract class Shape
{
    protected readonly IRenderer Renderer;

    protected Shape(IRenderer renderer) => Renderer = renderer;

    public abstract string Draw();
}

public sealed class Circle : Shape
{
    private readonly float _radius;

    public Circle(IRenderer renderer, float radius) : base(renderer) => _radius = radius;

    public override string Draw() => Renderer.RenderCircle(_radius);
}

public static class PatternDemo
{
    public static object Create()
    {
        var vectorCircle = new Circle(new VectorRenderer(), 12.5f);
        var rasterCircle = new Circle(new RasterRenderer(), 12.5f);

        return new
        {
            Pattern = "Bridge",
            Variant = "Abstraction-Implementation Separation",
            Vector = vectorCircle.Draw(),
            Raster = rasterCircle.Draw()
        };
    }
}
