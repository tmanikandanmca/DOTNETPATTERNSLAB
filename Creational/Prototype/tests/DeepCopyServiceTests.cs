using Prototype.DeepCopy.Api;

namespace Prototype.UnitTests;

public class DeepCopyServiceTests
{
    [Test]
    public void DeepClone_CopiesValues_ButReturnsDifferentReferenceGraph()
    {
        var source = new CustomerProfile
        {
            Name = "Ravi",
            Address = new Address { City = "Madurai" }
        };

        var clone = source.DeepClone();

        Assert.Multiple(() =>
        {
            Assert.That(clone, Is.Not.SameAs(source));
            Assert.That(clone.Name, Is.EqualTo(source.Name));
            Assert.That(clone.Address.City, Is.EqualTo(source.Address.City));
            Assert.That(clone.Address, Is.Not.SameAs(source.Address));
        });
    }

    [Test]
    public void ChangingClone_DoesNotAffectOriginal_ForRootAndNestedState()
    {
        var source = new CustomerProfile
        {
            Name = "Ravi",
            Address = new Address { City = "Madurai" }
        };

        var clone = source.DeepClone();
        clone.Name = "Ravi Clone";
        clone.Address.City = "Coimbatore";

        Assert.Multiple(() =>
        {
            Assert.That(source.Name, Is.EqualTo("Ravi"));
            Assert.That(source.Address.City, Is.EqualTo("Madurai"));
            Assert.That(clone.Name, Is.EqualTo("Ravi Clone"));
            Assert.That(clone.Address.City, Is.EqualTo("Coimbatore"));
        });
    }
}
