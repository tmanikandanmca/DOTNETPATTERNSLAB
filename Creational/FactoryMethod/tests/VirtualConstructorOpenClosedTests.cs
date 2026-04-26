using FactoryMethod.VirtualConstructor.Api;

namespace FactoryMethod.UnitTests;

public class VirtualConstructorOpenClosedTests
{
    [Test]
    public void ExistingCaller_WorksWithNewProductType_WithoutCallerChanges()
    {
        MessageCreator existingCaller = new MarkdownMessageCreator();

        var output = existingCaller.Compose("release notes");

        Assert.That(output, Is.EqualTo("**release notes**"));
    }

    private sealed class MarkdownFormatter : IMessageFormatter
    {
        public string Format(string value) => $"**{value}**";
    }

    private sealed class MarkdownMessageCreator : MessageCreator
    {
        protected override IMessageFormatter CreateFormatter() => new MarkdownFormatter();
    }
}
