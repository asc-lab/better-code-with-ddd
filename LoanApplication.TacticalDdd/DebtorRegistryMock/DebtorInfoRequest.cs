namespace DebtorRegistryMock;

public record DebtorInfo(string Pesel, List<Debt> Debts);

public record Debt(decimal Amount);