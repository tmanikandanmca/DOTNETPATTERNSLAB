using System.Reflection;
using AbstractFactory.FactoryOfFactories.Api;

namespace AbstractFactory.UnitTests;

public class FactoryOfFactoriesChecklistTests
{
    [Test]
    public void NotificationService_DependsOnlyOnAbstraction()
    {
        var constructor = typeof(NotificationService).GetConstructors().Single();
        var parameters = constructor.GetParameters();

        Assert.Multiple(() =>
        {
            Assert.That(parameters, Has.Length.EqualTo(1));
            Assert.That(parameters[0].ParameterType, Is.EqualTo(typeof(INotificationSuiteFactory)));
        });
    }

    [Test]
    public void ConcreteFactories_ReturnMatchingProductFamilyMembers()
    {
        INotificationSuiteFactory email = new EmailNotificationSuiteFactory();
        INotificationSuiteFactory sms = new SmsNotificationSuiteFactory();

        var emailSender = email.CreateSender();
        var emailFormatter = email.CreateFormatter();
        var smsSender = sms.CreateSender();
        var smsFormatter = sms.CreateFormatter();

        Assert.Multiple(() =>
        {
            Assert.That(email.Family, Is.EqualTo("Email"));
            Assert.That(emailSender.Channel, Is.EqualTo("Email"));
            Assert.That(emailFormatter.Format("hello"), Does.StartWith("[EMAIL]"));

            Assert.That(sms.Family, Is.EqualTo("Sms"));
            Assert.That(smsSender.Channel, Is.EqualTo("Sms"));
            Assert.That(smsFormatter.Format("hello"), Does.StartWith("[SMS]"));
        });
    }

    [Test]
    public void NewChannelFactory_CanBeAdded_WithoutChangingNotificationServiceLogic()
    {
        var service = new NotificationService(new PushNotificationSuiteFactory());
        var result = service.Send("release-ready");

        var payloadProperty = result.GetType().GetProperty("Payload", BindingFlags.Instance | BindingFlags.Public);

        Assert.That(payloadProperty, Is.Not.Null);
        Assert.That(payloadProperty!.GetValue(result)?.ToString(), Does.StartWith("PUSH:[PUSH]"));
    }

    [Test]
    public void Provider_WithInvalidChannel_FailsFastWithClearException()
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            NotificationSuiteFactoryProvider.Create("fax"));

        Assert.That(ex!.Message, Does.Contain("Unsupported channel"));
    }

    private sealed class PushNotificationSuiteFactory : INotificationSuiteFactory
    {
        public string Family => "Push";

        public INotificationSender CreateSender() => new PushSender();

        public IMessageFormatter CreateFormatter() => new PushFormatter();
    }

    private sealed class PushSender : INotificationSender
    {
        public string Channel => "Push";

        public string Send(string message) => $"PUSH:{message}";
    }

    private sealed class PushFormatter : IMessageFormatter
    {
        public string Format(string message) => $"[PUSH] {message}";
    }
}
