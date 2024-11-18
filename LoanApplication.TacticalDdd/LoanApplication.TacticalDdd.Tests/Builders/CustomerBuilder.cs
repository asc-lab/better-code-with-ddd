using LoanApplication.TacticalDdd.DomainModel;

namespace LoanApplication.TacticalDdd.Tests.Builders;

public class CustomerBuilder
{
    private Address address = new("PL", "00-001", "Warsaw", "Zielona 6");
    private int age;
    private MonetaryAmount income = new(4_500M);
    private Name name = new("Jan", "B");
    private NationalIdentifier nationalIdentifier = new("11111111111");

    public CustomerBuilder WithIdentifier(string nationalId)
    {
        nationalIdentifier = new NationalIdentifier(nationalId);
        return this;
    }

    public CustomerBuilder WithName(string first, string last)
    {
        name = new Name(first, last);
        return this;
    }

    public CustomerBuilder WithAge(int age)
    {
        this.age = age;
        return this;
    }

    public CustomerBuilder WithIncome(decimal income)
    {
        this.income = new MonetaryAmount(income);
        return this;
    }

    public CustomerBuilder WithAddress(string country, string zip, string city, string street)
    {
        address = new Address(country, zip, city, street);
        return this;
    }

    public Customer Build()
    {
        return new Customer
        (
            nationalIdentifier,
            name,
            SysTime.CurrentDate().AddYears(-1 * age),
            income,
            address
        );
    }
}