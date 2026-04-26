using FactoryMethod.SimpleFactory.Api;

namespace FactoryMethod.UnitTests;

public class SimpleFactoryValidationTests
{
    [Test]
    public void Create_ReturnsDifferentConcreteTypes_BasedOnInput()
    {
        var pdf = StaticRendererFactory.Create("pdf");
        var text = StaticRendererFactory.Create("text");

        Assert.Multiple(() =>
        {
            Assert.That(pdf, Is.InstanceOf<PdfRenderer>());
            Assert.That(text, Is.InstanceOf<TextRenderer>());
            Assert.That(pdf.GetType(), Is.Not.EqualTo(text.GetType()));
        });
    }

    [Test]
    public void Create_ReturnType_IsInterface_NotConcreteType()
    {
        var returnType = typeof(StaticRendererFactory)
            .GetMethod(nameof(StaticRendererFactory.Create))!
            .ReturnType;

        Assert.That(returnType, Is.EqualTo(typeof(IRenderer)));
    }

    [Test]
    public void Create_ThrowsClearError_ForUnknownInput()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => StaticRendererFactory.Create("xml"));

        Assert.Multiple(() =>
        {
            Assert.That(ex!.ParamName, Is.EqualTo("output"));
            Assert.That(ex.Message, Does.Contain("Unknown renderer type"));
        });
    }
}
