using System.Reflection;
using Structural.Bridge.RefinedAbstractionVsConcreteImplementor.Api;

namespace Structural.Bridge.Tests;

public class RefinedAbstractionVsConcreteImplementorChecklistTests
{
    [Test]
    public void Notification_HoldsImplementorViaInterfaceField()
    {
        var channelField = typeof(Notification).GetField("Channel", BindingFlags.Instance | BindingFlags.NonPublic);

        Assert.That(channelField, Is.Not.Null);
        Assert.That(channelField!.FieldType, Is.EqualTo(typeof(IChannel)));
    }

    [Test]
    public void NewImplementor_CanBeUsedWithoutChangingNotificationAbstractions()
    {
        var notification = new StandardNotification(new PushChannel());

        Assert.That(notification.Dispatch("hello"), Is.EqualTo("PUSH::hello"));
    }

    [Test]
    public void NewRefinedAbstraction_CanReuseExistingImplementors()
    {
        Notification notification = new UrgentNotification(new EmailChannel());

        Assert.That(notification.Dispatch("system alert"), Is.EqualTo("EMAIL::URGENT::system alert"));
    }

    [Test]
    public void CompositionRoot_KnowsBothConcreteSides()
    {
        var demo = PatternDemo.Create();
        var demoType = demo.GetType();

        Assert.Multiple(() =>
        {
            Assert.That(demoType.GetProperty("Pattern")!.GetValue(demo), Is.EqualTo("Bridge"));
            Assert.That(demoType.GetProperty("Variant")!.GetValue(demo), Is.EqualTo("Refined Abstraction vs Concrete Implementor"));
            Assert.That(demoType.GetProperty("Standard")!.GetValue(demo), Is.EqualTo("EMAIL::Build succeeded"));
            Assert.That(demoType.GetProperty("Priority")!.GetValue(demo), Is.EqualTo("SMS::[PRIORITY] API down"));
        });
    }

    private sealed class PushChannel : IChannel
    {
        public string Send(string message) => $"PUSH::{message}";
    }

    private sealed class UrgentNotification : Notification
    {
        public UrgentNotification(IChannel channel) : base(channel) { }

        public override string Dispatch(string message) => Channel.Send($"URGENT::{message}");
    }
}