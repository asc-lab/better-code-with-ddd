using System.Text;
using Dapper;
using LoanApplication.TacticalDdd.Application.Api;
using Npgsql;

namespace LoanApplication.TacticalDdd.ReadModel;

public class LoanApplicationFinder(string connectionString)
{
    public IList<LoanApplicationInfoDto> FindLoadApplication(LoanApplicationSearchCriteriaDto criteria)
    {
        using var cn = new NpgsqlConnection(connectionString);
            
        cn.Open();
        return cn
            .Query<LoanApplicationInfoDto>(BuildSearchQuery(criteria), criteria)
            .ToList();
    }

    private static string BuildSearchQuery(LoanApplicationSearchCriteriaDto criteria)
    {
        var query = new StringBuilder();
        query.AppendLine
        ("""
            SELECT 
                number AS Number, 
                status AS Status,
            CustomerFirstName || ' ' || CustomerLastName AS CustomerName, 
            decisionDate AS DecisionDate, 
            LoanAmount AS LoanAmount, 
            DecisionBy AS DecisionBy
            FROM loan_details_view
            WHERE 1=1 
        """);
            
        if (!string.IsNullOrWhiteSpace(criteria.ApplicationNumber))
        {
            query.AppendLine(" AND number = :ApplicationNumber");
        }
            
        if (!string.IsNullOrWhiteSpace(criteria.CustomerNationalIdentifier))
        {
            query.AppendLine(" AND customerNationalIdentifier = :CustomerNationalIdentifier");
        }
            
        if (!string.IsNullOrWhiteSpace(criteria.DecisionBy))
        {
            query.AppendLine(" AND decisionBy = :DecisionBy");
        }
            
        if (!string.IsNullOrWhiteSpace(criteria.RegisteredBy))
        {
            query.AppendLine(" AND registeredBy = :RegisteredBy");
        }

        return query.ToString();
    }
        
    public LoanApplicationDto GetLoanApplication(string applicationNumber)
    {
        using var cn = new NpgsqlConnection(connectionString);
            
        cn.Open();
        return cn
            .Query<LoanApplicationDto, AddressDto, AddressDto, LoanApplicationDto>
            (
                BuildSelectDetailsQuery(),
                (loan, customerAddress, propertyAddress) =>
                {
                    loan = loan with { CustomerAddress = customerAddress };
                    loan = loan with { PropertyAddress = propertyAddress };
                    return loan;
                },
                new { ApplicationNumber = applicationNumber },
                splitOn: "Country,Country")
            .FirstOrDefault();
    }

    private static string BuildSelectDetailsQuery()
    {
        return """
            SELECT 
                
                number AS Number, 
                status AS Status, 
                score AS Score, 
                    
                customerNationalIdentifier AS CustomerNationalIdentifier, 
                customerFirstName AS CustomerFirstName, 
                customerLastName AS CustomerLastName, 
                customerBirthdate AS CustomerBirthdate, 
                customerMonthlyIncome AS CustomerMonthlyIncome, 

                propertyValue AS PropertyValue, 

                loanAmount AS LoanAmount, 
                loanNumberOfYears AS LoanNumberOfYears, 
                interestRate AS InterestRate, 

                registeredBy AS RegisteredBy, 
                registrationDate AS RegistrationDate, 

                decisionDate AS DecisionDate, 
                decisionBy AS DecisionBy, 
                    
                customerAddress_country AS Country, 
                customerAddress_zipCode AS ZipCode, 
                customerAddress_city AS City, 
                customerAddress_street AS Street, 
                    
                propertyAddress_country AS Country, 
                propertyAddress_zipCode AS ZipCode, 
                propertyAddress_city AS City, 
                propertyAddress_street AS Street 

            FROM loan_details_view 
            WHERE number = :ApplicationNumber 
        """;
    }
}