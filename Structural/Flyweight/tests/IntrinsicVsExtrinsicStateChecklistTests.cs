using Structural.Flyweight.IntrinsicVsExtrinsicState.Api;

namespace Flyweight.UnitTests;

public class IntrinsicVsExtrinsicStateChecklistTests
{
    [Test]
    public void FactoryReturnsIdenticalInstance_ForSameIntrinsicState()
    {
        var factory = new GlyphFactory();

        var first = factory.Get('A', "Arial");
        var second = factory.Get('A', "Arial");
        var third = factory.Get('B', "Arial");

        Assert.Multiple(() =>
        {
            Assert.That(first, Is.SameAs(second), "Repeated Get with same intrinsic state must return same instance");
            Assert.That(first, Is.Not.SameAs(third), "Different intrinsic state should return different instance");
            Assert.That(factory.SharedInstances, Is.EqualTo(2), "Factory should have cached 2 unique flyweights");
        });
    }

    [Test]
    public void ExtrinsicState_NotStoredOnFlyweight_PassedByCallerOnly()
    {
        var factory = new GlyphFactory();
        var glyph = factory.Get('X', "Courier");

        var render1 = glyph.Draw(10, 20, "red");
        var render2 = glyph.Draw(50, 100, "blue");

        Assert.Multiple(() =>
        {
            Assert.That(render1, Does.Contain("(10,20)"), "First call position should be in output");
            Assert.That(render1, Does.Contain("red"), "First call color should be in output");
            Assert.That(render2, Does.Contain("(50,100)"), "Second call position should be in output");
            Assert.That(render2, Does.Contain("blue"), "Second call color should be in output");
            Assert.That(render1, Is.Not.EqualTo(render2), "Same flyweight, different extrinsic state produces different output");
        });
    }

    [Test]
    public void ReferenceEqualityVerified_ReusedFlyweightsAreSameInstance()
    {
        var factory = new GlyphFactory();
        var chars = "AABBAA";
        var instances = new List<GlyphFlyweight>();

        foreach (var ch in chars)
        {
            instances.Add(factory.Get(ch, "TimesNewRoman"));
        }

        Assert.Multiple(() =>
        {
            Assert.That(instances[0], Is.SameAs(instances[1]), "First A matches second A (same instance)");
            Assert.That(instances[2], Is.SameAs(instances[3]), "First B matches second B (same instance)");
            Assert.That(instances[0], Is.SameAs(instances[4]), "First A matches fifth char A (same instance)");
            Assert.That(instances[0], Is.SameAs(instances[5]), "First A matches last A (same instance)");
            Assert.That(factory.SharedInstances, Is.EqualTo(2), "Only 2 unique glyphs created despite 6 requests");
        });
    }

    [Test]
    public void ImmutabilityEnforced_FlyweightStateNeverChanges()
    {
        var factory = new GlyphFactory();
        var glyph = factory.Get('Z', "Helvetica");

        var originalSymbol = glyph.Symbol;
        var originalFont = glyph.FontFamily;

        var render1 = glyph.Draw(0, 0, "green");
        var render2 = glyph.Draw(100, 200, "purple");

        Assert.Multiple(() =>
        {
            Assert.That(glyph.Symbol, Is.EqualTo(originalSymbol), "Flyweight symbol must remain immutable");
            Assert.That(glyph.FontFamily, Is.EqualTo(originalFont), "Flyweight font must remain immutable");
        });
    }

    [Test]
    public void CacheHitsReduceObjectCreation_LargeScaleReuse()
    {
        var factory = new GlyphFactory();

        var initialCount = factory.SharedInstances;

        for (int i = 0; i < 1000; i++)
        {
            var ch = (char)('A' + (i % 5));
            var glyph = factory.Get(ch, "Monospace");
            _ = glyph.Draw(i * 10, i * 5, i % 2 == 0 ? "black" : "white");
        }

        Assert.That(factory.SharedInstances, Is.EqualTo(5), 
            "Despite 1000 requests, only 5 unique flyweights should exist");
    }
}
