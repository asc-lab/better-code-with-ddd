namespace LoanApplication.TacticalDdd.Application.Api;

public record LoanApplicationSearchCriteriaDto
(
    string ApplicationNumber,
    string CustomerNationalIdentifier,
    string DecisionBy,
    string RegisteredBy
);