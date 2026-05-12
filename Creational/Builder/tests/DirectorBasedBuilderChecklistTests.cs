using Builder.DirectorBasedBuilder.Api;

namespace Builder.UnitTests;

public class DirectorBasedBuilderChecklistTests
{
    [Test]
    public void DirectorRecipe_ProducesConsistentResultsAcrossCalls()
    {
        var director = new SandwichDirector();

        var first = director.CreateClub(new SandwichBuilder());
        var second = director.CreateClub(new SandwichBuilder());

        Assert.Multiple(() =>
        {
            Assert.That(first, Is.Not.SameAs(second));
            Assert.That(first, Is.EqualTo(second));
            Assert.That(first.Bread, Is.EqualTo("Whole Grain"));
            Assert.That(first.Main, Is.EqualTo("Chicken"));
            Assert.That(first.HasSalad, Is.True);
            Assert.That(first.HasSauce, Is.True);
        });
    }

    [Test]
    public void BuilderSubType_ProducesIndependentObjectsWhenReusedViaDirector()
    {
        var director = new SandwichDirector();
        var builder = new SandwichBuilder();

        var first = director.CreateClub(builder);
        var second = director.CreateClub(builder);

        Assert.Multiple(() =>
        {
            Assert.That(first, Is.Not.SameAs(second));
            Assert.That(first, Is.EqualTo(second));
        });
    }
}
