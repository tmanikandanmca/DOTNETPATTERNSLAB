using Prototype.ShallowCopy.Api;

namespace Prototype.UnitTests;

public class ShallowCopyServiceTests
{
    [Test]
    public void ShallowClone_CopiesValues_ButReturnsDifferentRootReference()
    {
        var source = new CustomerProfile
        {
            Name = "Asha",
            Address = new Address { City = "Chennai" }
        };

        var clone = source.ShallowClone();

        Assert.Multiple(() =>
        {
            Assert.That(clone, Is.Not.SameAs(source));
            Assert.That(clone.Name, Is.EqualTo(source.Name));
            Assert.That(clone.Address.City, Is.EqualTo(source.Address.City));
        });
    }

    [Test]
    public void ChangingCloneRootState_DoesNotAffectOriginalRootState()
    {
        var source = new CustomerProfile
        {
            Name = "Asha",
            Address = new Address { City = "Chennai" }
        };

        var clone = source.ShallowClone();
        clone.Name = "Asha Clone";

        Assert.Multiple(() =>
        {
            Assert.That(source.Name, Is.EqualTo("Asha"));
            Assert.That(clone.Name, Is.EqualTo("Asha Clone"));
        });
    }

    [Test]
    public void ShallowClone_SharesNestedReference_BetweenOriginalAndClone()
    {
        var source = new CustomerProfile
        {
            Name = "Asha",
            Address = new Address { City = "Chennai" }
        };

        var clone = source.ShallowClone();
        clone.Address.City = "Bengaluru";

        Assert.Multiple(() =>
        {
            Assert.That(clone.Address, Is.SameAs(source.Address));
            Assert.That(source.Address.City, Is.EqualTo("Bengaluru"));
        });
    }
}
