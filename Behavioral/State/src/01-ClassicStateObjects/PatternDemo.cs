namespace Behavioral.State.ClassicStateObjects.Api;

public interface IOrderState
{
    string Name { get; }
    string Pay(OrderContext context);
    string Ship(OrderContext context);
}

public sealed class NewOrderState : IOrderState
{
    public string Name => "New";

    public string Pay(OrderContext context)
    {
        context.SetState(new PaidOrderState());
        return "Order paid";
    }

    public string Ship(OrderContext context) => "Cannot ship a new order";
}

public sealed class PaidOrderState : IOrderState
{
    public string Name => "Paid";

    public string Pay(OrderContext context) => "Order already paid";

    public string Ship(OrderContext context)
    {
        context.SetState(new ShippedOrderState());
        return "Order shipped";
    }
}

public sealed class ShippedOrderState : IOrderState
{
    public string Name => "Shipped";

    public string Pay(OrderContext context) => "Order already shipped";

    public string Ship(OrderContext context) => "Order already shipped";
}

public sealed class OrderContext
{
    public IOrderState State { get; private set; } = new NewOrderState();

    public void SetState(IOrderState state) => State = state;

    public string Pay() => State.Pay(this);

    public string Ship() => State.Ship(this);
}

public sealed class TrafficLightMachine
{
    public string State { get; private set; } = "Red";

    public string Advance() => State = State switch
    {
        "Red" => "Green",
        "Green" => "Yellow",
        _ => "Red"
    };
}

public static class StateDemo
{
    public static object Create()
    {
        var order = new OrderContext();
        var orderSteps = new List<string>
        {
            order.State.Name,
            order.Pay(),
            order.State.Name,
            order.Ship(),
            order.State.Name
        };

        var light = new TrafficLightMachine();
        var traffic = new[] { light.State, light.Advance(), light.Advance(), light.Advance() };

        return new
        {
            Pattern = "State",
            ClassicStateObjects = orderSteps,
            EnumSwitchStateMachine = traffic
        };
    }
}
