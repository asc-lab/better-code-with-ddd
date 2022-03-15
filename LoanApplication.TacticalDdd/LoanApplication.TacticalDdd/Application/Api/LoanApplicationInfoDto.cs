namespace LoanApplication.TacticalDdd.Application.Api;

public record LoanApplicationInfoDto
(
    string Number,
    string Status,
    string CustomerName,
    DateTime? DecisionDate,
    decimal LoanAmount,
    string DecisionBy
);