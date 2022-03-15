using LoanApplication.TacticalDdd.DomainModel;

namespace LoanApplication.TacticalDdd.Tests.Builders;

public class PropertyBuilder
{
    private MonetaryAmount value = new MonetaryAmount(400_000M);
    private Address address = new Address("PL","00-001","Warsaw","Zielona 6");

    public PropertyBuilder WithValue(decimal propertyValue)
    {
        value = new MonetaryAmount(propertyValue);
        return this;
    }

    public PropertyBuilder WithAddress(string country,string zip,string city,string street)
    {
        this.address = new Address(country,zip,city,street);
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