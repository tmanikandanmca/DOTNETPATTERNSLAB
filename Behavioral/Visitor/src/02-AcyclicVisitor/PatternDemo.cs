namespace Behavioral.Visitor.AcyclicVisitor.Api;

public interface IVisitable
{
    string Accept(object visitor);
}

public interface IVisitCircle
{
    string Visit(Circle circle);
}

public interface IVisitSquare
{
    string Visit(Square square);
}

public sealed record Circle(int Radius) : IVisitable
{
    public string Accept(object visitor)
        => visitor is IVisitCircle typed ? typed.Visit(this) : "No circle visitor";
}

public sealed record Square(int Side) : IVisitable
{
    public string Accept(object visitor)
        => visitor is IVisitSquare typed ? typed.Visit(this) : "No square visitor";
}

public sealed class MetricsVisitor : IVisitCircle, IVisitSquare
{
    public string Visit(Circle circle) => $"Circle perimeter: {2 * Math.PI * circle.Radius:F2}";
    public string Visit(Square square) => $"Square perimeter: {4 * square.Side}";
}

public static class AcyclicVisitorDemo
{
    public static object Create()
    {
        IVisitable[] shapes = [new Circle(3), new Square(5)];
        var visitor = new MetricsVisitor();

        return new
        {
            Pattern = "Visitor",
            Variant = "Acyclic Visitor",
            Results = shapes.Select(shape => shape.Accept(visitor)).ToArray()
        };
    }
}
