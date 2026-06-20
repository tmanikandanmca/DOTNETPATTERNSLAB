using Behavioral.State.ClassicStateObjects.Api;

namespace Behavioral.State.Tests;

public class ClassicStateObjectsChecklistTests
{
    [Test]
    public void Context_DelegatesToCurrentState_WithoutHardcodedBranches()
    {
        var probe = new ProbeOrderState();
        var context = new OrderContext();
        context.SetState(probe);

        var payResult = context.Pay();
        var shipResult = context.Ship();

        Assert.Multiple(() =>
        {
            Assert.That(payResult, Is.EqualTo("probe-pay"));
            Assert.That(shipResult, Is.EqualTo("probe-ship"));
            Assert.That(probe.PayCalls, Is.EqualTo(1));
            Assert.That(probe.ShipCalls, Is.EqualTo(1));
        });
    }

    [Test]
    public void StateClasses_KeepStateSpecificBehavior_CloseToTheState()
    {
        var newState = new NewOrderState();
        var paidState = new PaidOrderState();
        var shippedState = new ShippedOrderState();
        var context = new OrderContext();

        Assert.Multiple(() =>
        {
            Assert.That(newState.Name, Is.EqualTo("New"));
            Assert.That(newState.Ship(context), Is.EqualTo("Cannot ship a new order"));
            Assert.That(paidState.Name, Is.EqualTo("Paid"));
            Assert.That(paidState.Pay(context), Is.EqualTo("Order already paid"));
            Assert.That(shippedState.Name, Is.EqualTo("Shipped"));
            Assert.That(shippedState.Pay(context), Is.EqualTo("Order already shipped"));
        });
    }

    [Test]
    public void Transitions_AreExplicit_AndMoveTheContextThroughTheExpectedStates()
    {
        var context = new OrderContext();

        var payResult = context.Pay();
        var paidStateName = context.State.Name;
        var shipResult = context.Ship();
        var shippedStateName = context.State.Name;

        Assert.Multiple(() =>
        {
            Assert.That(payResult, Is.EqualTo("Order paid"));
            Assert.That(paidStateName, Is.EqualTo("Paid"));
            Assert.That(shipResult, Is.EqualTo("Order shipped"));
            Assert.That(shippedStateName, Is.EqualTo("Shipped"));
        });
    }

    [Test]
    public void InvalidTransitions_AreHandledClearly_AndKeepTheContextStable()
    {
        var context = new OrderContext();

        var shipResult = context.Ship();
        var stateAfterShip = context.State.Name;

        Assert.Multiple(() =>
        {
            Assert.That(shipResult, Is.EqualTo("Cannot ship a new order"));
            Assert.That(stateAfterShip, Is.EqualTo("New"));
        });
    }

    [Test]
    public void ConcreteStates_AreSwappable_ThroughTheSharedInterface()
    {
        var context = new OrderContext();
        IOrderState state = new NewOrderState();

        context.SetState(state);
        var payResult = context.Pay();

        state = new PaidOrderState();
        context.SetState(state);
        var shipResult = context.Ship();

        Assert.Multiple(() =>
        {
            Assert.That(payResult, Is.EqualTo("Order paid"));
            Assert.That(shipResult, Is.EqualTo("Order shipped"));
        });
    }

    private sealed class ProbeOrderState : IOrderState
    {
        public string Name => "Probe";

        public int PayCalls { get; private set; }

        public int ShipCalls { get; private set; }

        public string Pay(OrderContext context)
        {
            PayCalls++;
            return "probe-pay";
        }

        public string Ship(OrderContext context)
        {
            ShipCalls++;
            return "probe-ship";
        }
    }
}