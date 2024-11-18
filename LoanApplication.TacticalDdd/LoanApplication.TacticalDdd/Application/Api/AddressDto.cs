namespace LoanApplication.TacticalDdd.Application.Api;

public record AddressDto
(
    string Country,
    string ZipCode,
    string City,
    string Street
);