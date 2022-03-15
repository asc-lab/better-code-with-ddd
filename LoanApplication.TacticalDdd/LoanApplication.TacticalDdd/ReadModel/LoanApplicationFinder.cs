using System.Text;
using Dapper;
using LoanApplication.TacticalDdd.Application.Api;
using Npgsql;

namespace LoanApplication.TacticalDdd.ReadModel;

public class LoanApplicationFinder
{
    private readonly string connectionString;
        
    public LoanApplicationFinder(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public IList<LoanApplicationInfoDto> FindLoadApplication(LoanApplicationSearchCriteriaDto criteria)
    {
        using var cn = new NpgsqlConnection(connectionString);
            
        cn.Open();
        return cn
            .Query<LoanApplicationInfoDto>(BuildSearchQuery(criteria), criteria)
            .ToList();
    }

    private string BuildSearchQuery(LoanApplicationSearchCriteriaDto criteria)
    {
        var query = new StringBuilder();
        query.AppendLine("SELECT ");
        query.AppendLine("number AS Number, ");
        query.AppendLine("status AS Status, ");
        query.AppendLine("CustomerFirstName || ' ' || CustomerLastName AS CustomerName, ");
        query.AppendLine("decisionDate AS DecisionDate, ");
        query.AppendLine("LoanAmount AS LoanAmount, ");
        query.AppendLine("DecisionBy AS DecisionBy");
        query.AppendLine("FROM loan_details_view");
        query.AppendLine("WHERE 1=1 ");
            
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

    private string BuildSelectDetailsQuery()
    {
        var query = new StringBuilder();
        query.AppendLine("SELECT ");
            
        query.AppendLine("number AS Number, ");
        query.AppendLine("status AS Status, ");
        query.AppendLine("score AS Score, ");
            
        query.AppendLine("customerNationalIdentifier AS CustomerNationalIdentifier, ");
        query.AppendLine("customerFirstName AS CustomerFirstName, ");
        query.AppendLine("customerLastName AS CustomerLastName, ");
        query.AppendLine("customerBirthdate AS CustomerBirthdate, ");
        query.AppendLine("customerMonthlyIncome AS CustomerMonthlyIncome, ");

        query.AppendLine("propertyValue AS PropertyValue, ");

        query.AppendLine("loanAmount AS LoanAmount, ");
        query.AppendLine("loanNumberOfYears AS LoanNumberOfYears, ");
        query.AppendLine("interestRate AS InterestRate, ");

        query.AppendLine("registeredBy AS RegisteredBy, ");
        query.AppendLine("registrationDate AS RegistrationDate, ");

        query.AppendLine("decisionDate AS DecisionDate, ");
        query.AppendLine("decisionBy AS DecisionBy, ");
            
        query.AppendLine("customerAddress_country AS Country, ");
        query.AppendLine("customerAddress_zipCode AS ZipCode, ");
        query.AppendLine("customerAddress_city AS City, ");
        query.AppendLine("customerAddress_street AS Street, ");
            
        query.AppendLine("propertyAddress_country AS Country, ");
        query.AppendLine("propertyAddress_zipCode AS ZipCode, ");
        query.AppendLine("propertyAddress_city AS City, ");
        query.AppendLine("propertyAddress_street AS Street ");

        query.AppendLine("FROM loan_details_view ");
        query.AppendLine("WHERE number = :ApplicationNumber ");
        return query.ToString();
    }
}