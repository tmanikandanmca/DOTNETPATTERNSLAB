using FactoryMethod.ParameterizedFactory.Api;

namespace FactoryMethod.UnitTests;

public class ParameterizedFactoryValidationTests
{
    [Test]
    public void Create_ReturnsDifferentConcreteTypes_BasedOnInput()
    {
        var email = NotifierFactory.Create("email", highPriority: false);
        var sms = NotifierFactory.Create("sms", highPriority: false);

        Assert.Multiple(() =>
        {
            Assert.That(email, Is.InstanceOf<EmailNotifier>());
            Assert.That(sms, Is.InstanceOf<SmsNotifier>());
            Assert.That(email.GetType(), Is.Not.EqualTo(sms.GetType()));
        });
    }

    [Test]
    public void Create_ReturnType_IsInterface_NotConcreteType()
    {
        var returnType = typeof(NotifierFactory)
            .GetMethod(nameof(NotifierFactory.Create), new[] { typeof(string), typeof(bool) })!
            .ReturnType;

        Assert.That(returnType, Is.EqualTo(typeof(INotifier)));
    }

    [Test]
    public void Create_ThrowsClearError_ForUnknownInput()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            NotifierFactory.Create("push", highPriority: false));

        Assert.Multiple(() =>
        {
            Assert.That(ex!.ParamName, Is.EqualTo("channel"));
            Assert.That(ex.Message, Does.Contain("Unknown channel"));
        });
    }
}
