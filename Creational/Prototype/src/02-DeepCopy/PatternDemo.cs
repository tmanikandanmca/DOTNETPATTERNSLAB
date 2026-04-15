namespace Prototype.DeepCopy.Api;

public sealed class Address
{
    public string City { get; set; } = string.Empty;
}

public sealed class CustomerProfile
{
    public string Name { get; set; } = string.Empty;
    public Address Address { get; set; } = new();

    public CustomerProfile DeepClone()
        => new()
        {
            Name = Name,
            Address = new Address { City = Address.City }
        };
}

public static class DeepCopyDemo
{
    public static object Create()
    {
        var source = new CustomerProfile { Name = "Ravi", Address = new Address { City = "Madurai" } };
        var clone = source.DeepClone();
        clone.Address.City = "Coimbatore";

        return new
        {
            Pattern = "Prototype",
            Variant = "Deep Copy",
            SameNestedReference = ReferenceEquals(source.Address, clone.Address),
            OriginalCity = source.Address.City,
            CloneCity = clone.Address.City
        };
    }
}
