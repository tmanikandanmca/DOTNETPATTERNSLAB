using Builder.FluentBuilder.Api;

namespace Builder.UnitTests;

public class FluentBuilderChecklistTests
{
    [Test]
    public void Build_ReturnsIndependentObject_NotSharedWithBuilderState()
    {
        var builder = new ApiRequestBuilder()
            .WithEndpoint("/orders")
            .UsingMethod("POST")
            .AddHeader("x-request-id", "1");

        var first = builder.Build();
        builder.AddHeader("x-request-id", "2");
        var second = builder.Build();

        Assert.Multiple(() =>
        {
            Assert.That(first, Is.Not.SameAs(second));
            Assert.That(first.Headers["x-request-id"], Is.EqualTo("1"));
            Assert.That(second.Headers["x-request-id"], Is.EqualTo("2"));
        });
    }

    [Test]
    public void CustomizationViaBuilder_DoesNotMutatePrototypeRequest()
    {
        var prototype = new ApiRequest(
            "/orders",
            "POST",
            new Dictionary<string, string> { ["x-template"] = "true" });

        var built = new ApiRequestBuilder()
            .WithEndpoint(prototype.Endpoint)
            .UsingMethod(prototype.Method)
            .AddHeader("x-template", prototype.Headers["x-template"])
            .AddHeader("x-run", "42")
            .Build();

        Assert.Multiple(() =>
        {
            Assert.That(built, Is.Not.SameAs(prototype));
            Assert.That(built.Headers.ContainsKey("x-run"), Is.True);
            Assert.That(prototype.Headers.ContainsKey("x-run"), Is.False);
        });
    }
}
