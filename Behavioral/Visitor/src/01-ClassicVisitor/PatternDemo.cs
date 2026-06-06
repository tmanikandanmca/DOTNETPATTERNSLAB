namespace Behavioral.Visitor.ClassicVisitor.Api;

public interface IShape
{
    string Accept(IShapeVisitor visitor);
}

public interface IShapeVisitor
{
    string VisitCircle(Circle circle);
    string VisitRectangle(Rectangle rectangle);
}

public sealed record Circle(int Radius) : IShape
{
    public string Accept(IShapeVisitor visitor) => visitor.VisitCircle(this);
}

public sealed record Rectangle(int Width, int Height) : IShape
{
    public string Accept(IShapeVisitor visitor) => visitor.VisitRectangle(this);
}

public sealed class AreaVisitor : IShapeVisitor
{
    public string VisitCircle(Circle circle) => $"Circle area: {Math.Round(Math.PI * circle.Radius * circle.Radius, 2)}";
    public string VisitRectangle(Rectangle rectangle) => $"Rectangle area: {rectangle.Width * rectangle.Height}";
}

public sealed class ExportVisitor : IShapeVisitor
{
    public string VisitCircle(Circle circle) => $"Export circle r={circle.Radius}";
    public string VisitRectangle(Rectangle rectangle) => $"Export rectangle w={rectangle.Width} h={rectangle.Height}";
}

public static class VisitorDemo
{
    public static object Create()
    {
        IShape[] shapes = [new Circle(3), new Rectangle(4, 5)];
        IShapeVisitor areaVisitor = new AreaVisitor();
        IShapeVisitor exportVisitor = new ExportVisitor();

        return new
        {
            Pattern = "Visitor",
            Areas = shapes.Select(shape => shape.Accept(areaVisitor)).ToArray(),
            Exports = shapes.Select(shape => shape.Accept(exportVisitor)).ToArray()
        };
    }
}
