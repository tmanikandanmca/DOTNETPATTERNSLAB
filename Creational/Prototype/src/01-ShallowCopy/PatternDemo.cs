namespace Prototype.ShallowCopy.Api;

public sealed class Address
{
    public string City { get; set; } = string.Empty;
}

public sealed class CustomerProfile
{
    public string Name { get; set; } = string.Empty;
    public Address Address { get; set; } = new();

    public CustomerProfile ShallowClone() => (CustomerProfile)MemberwiseClone();
}

public static class ShallowCopyDemo
{
    public static object Create()
    {
        var source = new CustomerProfile { Name = "Asha", Address = new Address { City = "Chennai" } };
        var clone = source.ShallowClone();

        return new
        {
            Pattern = "Prototype",
            Variant = "Shallow Copy",
            SameNestedReference = ReferenceEquals(source.Address, clone.Address),
            OriginalCity = source.Address.City,
            CloneCity = clone.Address.City
        };
    }
}
