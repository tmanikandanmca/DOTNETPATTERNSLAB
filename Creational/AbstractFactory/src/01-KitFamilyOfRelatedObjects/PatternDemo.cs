namespace AbstractFactory.KitFamilyOfRelatedObjects.Api;

public interface IButton { string Label(); }
public interface ICard { string Style(); }

public interface IUiKitFactory
{
    string Family { get; }
    IButton CreateButton();
    ICard CreateCard();
}

public sealed class LightButton : IButton { public string Label() => "Light Button"; }
public sealed class LightCard : ICard { public string Style() => "Light Card"; }
public sealed class DarkButton : IButton { public string Label() => "Dark Button"; }
public sealed class DarkCard : ICard { public string Style() => "Dark Card"; }

public sealed class LightUiKitFactory : IUiKitFactory
{
    public string Family => "Light";
    public IButton CreateButton() => new LightButton();
    public ICard CreateCard() => new LightCard();
}

public sealed class DarkUiKitFactory : IUiKitFactory
{
    public string Family => "Dark";
    public IButton CreateButton() => new DarkButton();
    public ICard CreateCard() => new DarkCard();
}

public static class KitFamilyOfRelatedObjectsDemo
{
    public static object Create()
    {
        IUiKitFactory factory = new DarkUiKitFactory();
        return new
        {
            Pattern = "Abstract Factory",
            Variant = "Kit (family of related objects)",
            Family = factory.Family,
            Button = factory.CreateButton().Label(),
            Card = factory.CreateCard().Style()
        };
    }
}
