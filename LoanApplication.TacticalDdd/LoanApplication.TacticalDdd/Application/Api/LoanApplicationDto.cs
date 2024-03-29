﻿namespace LoanApplication.TacticalDdd.Application.Api;

public record LoanApplicationDto
(
     string Number,
     string Status,
     string Score,
     string CustomerNationalIdentifier,
     string CustomerFirstName,
     string CustomerLastName,
     DateTime CustomerBirthdate,
     decimal CustomerMonthlyIncome,
     AddressDto CustomerAddress,
     decimal PropertyValue,
     AddressDto PropertyAddress,
     decimal LoanAmount,
     int LoanNumberOfYears,
     decimal InterestRate,
     DateTime? DecisionDate,
     string DecisionBy,
     string RegisteredBy,
     DateTime RegistrationDate
)
{
     //this one is needed to allow dapper to create instance of it using reflection
     protected LoanApplicationDto() : this(default, default, default, default, default, default, default, default,
          default, default, default, default, default, default, default, default, default, default)
     {
     }
};