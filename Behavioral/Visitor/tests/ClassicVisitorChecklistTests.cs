using Behavioral.Visitor.ClassicVisitor.Api;

namespace Behavioral.Visitor.UnitTests;

public class ClassicVisitorChecklistTests
{
    [Test]
    public void Operations_AreAddable_WithoutChangingEveryElementClass()
    {
        IShape[] shapes = [new Circle(2), new Rectangle(3, 4)];
        IShapeVisitor visitor = new DescribeVisitor();

        var results = shapes.Select(shape => shape.Accept(visitor)).ToArray();

        Assert.That(results, Is.EqualTo(new[] { "Circle radius 2", "Rectangle 3x4" }));
    }

    [Test]
    public void EachElement_ExposesAClearAcceptMethod()
    {
        IShape circle = new Circle(3);
        IShape rectangle = new Rectangle(4, 5);
        IShapeVisitor visitor = new ExportVisitor();

        Assert.Multiple(() =>
        {
            Assert.That(circle.Accept(visitor), Is.EqualTo("Export circle r=3"));
            Assert.That(rectangle.Accept(visitor), Is.EqualTo("Export rectangle w=4 h=5"));
        });
    }

    [Test]
    public void Visitors_StayFocusedOnOneOperation()
    {
        IShape[] shapes = [new Circle(3), new Rectangle(4, 5)];

        var areaResults = shapes.Select(shape => shape.Accept(new AreaVisitor())).ToArray();
        var exportResults = shapes.Select(shape => shape.Accept(new ExportVisitor())).ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(areaResults, Is.EqualTo(new[] { "Circle area: 28.27", "Rectangle area: 20" }));
            Assert.That(exportResults, Is.EqualTo(new[] { "Export circle r=3", "Export rectangle w=4 h=5" }));
        });
    }

    [Test]
    public void Tests_VerifyTraversal_AndDispatchBehavior()
    {
        IShape[] shapes = [new Circle(1), new Rectangle(2, 3), new Circle(4)];
        var calls = new List<string>();

        var results = shapes.Select(shape => shape.Accept(new RecordingClassicVisitor(calls))).ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(results, Is.EqualTo(new[] { "circle:1", "rectangle:2x3", "circle:4" }));
            Assert.That(calls, Is.EqualTo(new[] { "Circle", "Rectangle", "Circle" }));
        });
    }

    [Test]
    public void Hierarchy_IsStableEnough_ToJustifyThePattern()
    {
        IShape[] shapes = [new Circle(5), new Rectangle(2, 6)];

        var exportResults = shapes.Select(shape => shape.Accept(new ExportVisitor())).ToArray();
        var descriptionResults = shapes.Select(shape => shape.Accept(new DescribeVisitor())).ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(exportResults, Has.Length.EqualTo(2));
            Assert.That(descriptionResults, Is.EqualTo(new[] { "Circle radius 5", "Rectangle 2x6" }));
        });
    }

    private sealed class DescribeVisitor : IShapeVisitor
    {
        public string VisitCircle(Circle circle) => $"Circle radius {circle.Radius}";

        public string VisitRectangle(Rectangle rectangle) => $"Rectangle {rectangle.Width}x{rectangle.Height}";
    }

    private sealed class RecordingClassicVisitor(IList<string> calls) : IShapeVisitor
    {
        public string VisitCircle(Circle circle)
        {
            calls.Add("Circle");
            return $"circle:{circle.Radius}";
        }

        public string VisitRectangle(Rectangle rectangle)
        {
            calls.Add("Rectangle");
            return $"rectangle:{rectangle.Width}x{rectangle.Height}";
        }
    }
}