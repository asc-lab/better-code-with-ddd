using LoanApplication.TacticalDdd.DomainModel;

namespace LoanApplication.TacticalDdd.Tests.Builders;

public class PropertyBuilder
{
    private Address address = new("PL", "00-001", "Warsaw", "Zielona 6");
    private MonetaryAmount value = new(400_000M);

    public PropertyBuilder WithValue(decimal propertyValue)
    {
        value = new MonetaryAmount(propertyValue);
        return this;
    }

    public PropertyBuilder WithAddress(string country, string zip, string city, string street)
    {
        address = new Address(country, zip, city, street);
        return this;
    }

    public Property Build()
    {
        return new Property
        (
            value,
            address
        );
    }
}