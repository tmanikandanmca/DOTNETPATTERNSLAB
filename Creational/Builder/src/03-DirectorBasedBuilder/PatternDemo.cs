namespace Builder.DirectorBasedBuilder.Api;

public sealed record Sandwich(string Bread, string Main, bool HasSalad, bool HasSauce);

public interface ISandwichBuilder
{
    void Reset();
    void UseBread(string bread);
    void AddMain(string main);
    void AddSalad();
    void AddSauce();
    Sandwich Build();
}

public sealed class SandwichBuilder : ISandwichBuilder
{
    private string _bread = "White";
    private string _main = "Veggie";
    private bool _hasSalad;
    private bool _hasSauce;

    public void Reset()
    {
        _bread = "White";
        _main = "Veggie";
        _hasSalad = false;
        _hasSauce = false;
    }

    public void UseBread(string bread) => _bread = bread;
    public void AddMain(string main) => _main = main;
    public void AddSalad() => _hasSalad = true;
    public void AddSauce() => _hasSauce = true;
    public Sandwich Build() => new(_bread, _main, _hasSalad, _hasSauce);
}

public sealed class SandwichDirector
{
    public Sandwich CreateClub(ISandwichBuilder builder)
    {
        builder.Reset();
        builder.UseBread("Whole Grain");
        builder.AddMain("Chicken");
        builder.AddSalad();
        builder.AddSauce();
        return builder.Build();
    }
}

public static class DirectorBasedBuilderDemo
{
    public static object Create()
    {
        var director = new SandwichDirector();
        var sandwich = director.CreateClub(new SandwichBuilder());

        return new
        {
            Pattern = "Builder",
            Variant = "Director-based Builder",
            sandwich.Bread,
            sandwich.Main,
            sandwich.HasSalad,
            sandwich.HasSauce
        };
    }
}
