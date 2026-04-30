namespace LoanApplication.TacticalDdd.Application.Api;

public record LoanApplicationInfoDto
(
    string Number,
    string Status,
    string CustomerName,
    DateOnly? DecisionDate,
    decimal LoanAmount,
    string DecisionBy
);