using Behavioral.Visitor.AcyclicVisitor.Api;

namespace Behavioral.Visitor.UnitTests;

public class AcyclicVisitorChecklistTests
{
    [Test]
    public void Operations_AreAddable_WithoutChangingEveryElementClass()
    {
        IVisitable[] shapes = [new Circle(2), new Square(3)];
        var visitor = new DescribeVisitor();

        var results = shapes.Select(shape => shape.Accept(visitor)).ToArray();

        Assert.That(results, Is.EqualTo(new[] { "Circle radius 2", "Square side 3" }));
    }

    [Test]
    public void EachElement_ExposesAClearAcceptMethod()
    {
        IVisitable circle = new Circle(3);
        IVisitable square = new Square(5);
        var visitor = new MetricsVisitor();

        Assert.Multiple(() =>
        {
            Assert.That(circle.Accept(visitor), Is.EqualTo("Circle perimeter: 18.85"));
            Assert.That(square.Accept(visitor), Is.EqualTo("Square perimeter: 20"));
        });
    }

    [Test]
    public void Visitors_StayFocusedOnOneOperation()
    {
        IVisitable[] shapes = [new Circle(3), new Square(5)];

        var metrics = shapes.Select(shape => shape.Accept(new MetricsVisitor())).ToArray();
        var descriptions = shapes.Select(shape => shape.Accept(new DescribeVisitor())).ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(metrics, Is.EqualTo(new[] { "Circle perimeter: 18.85", "Square perimeter: 20" }));
            Assert.That(descriptions, Is.EqualTo(new[] { "Circle radius 3", "Square side 5" }));
        });
    }

    [Test]
    public void Tests_VerifyTraversal_AndDispatchBehavior()
    {
        IVisitable[] shapes = [new Square(2), new Circle(1), new Square(4)];
        var calls = new List<string>();
        var visitor = new RecordingAcyclicVisitor(calls);

        var results = shapes.Select(shape => shape.Accept(visitor)).ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(results, Is.EqualTo(new[] { "square:2", "circle:1", "square:4" }));
            Assert.That(calls, Is.EqualTo(new[] { "Square", "Circle", "Square" }));
        });
    }

    [Test]
    public void Hierarchy_IsStableEnough_ToJustifyThePattern()
    {
        IVisitable[] shapes = [new Circle(6), new Square(2)];

        var metrics = shapes.Select(shape => shape.Accept(new MetricsVisitor())).ToArray();
        var descriptions = shapes.Select(shape => shape.Accept(new DescribeVisitor())).ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(metrics, Has.Length.EqualTo(2));
            Assert.That(descriptions, Is.EqualTo(new[] { "Circle radius 6", "Square side 2" }));
        });
    }

    private sealed class DescribeVisitor : IVisitCircle, IVisitSquare
    {
        public string Visit(Circle circle) => $"Circle radius {circle.Radius}";

        public string Visit(Square square) => $"Square side {square.Side}";
    }

    private sealed class RecordingAcyclicVisitor(IList<string> calls) : IVisitCircle, IVisitSquare
    {
        public string Visit(Circle circle)
        {
            calls.Add("Circle");
            return $"circle:{circle.Radius}";
        }

        public string Visit(Square square)
        {
            calls.Add("Square");
            return $"square:{square.Side}";
        }
    }
}