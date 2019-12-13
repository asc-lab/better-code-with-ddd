using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LoanApplication.TacticalDdd.Application.Api;
using MediatR;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace LoanApplication.TacticalDdd.ReadModel
{
    public class GetLoanApplicationByNumber
    {
        public class Query : IRequest<LoanApplicationDto>
        {
            public string ApplicationNumber { get; set; }
        }
        
        public class Handler : IRequestHandler<Query, LoanApplicationDto>
        {
            private readonly string connectionString;
            
            public Handler(IConfiguration configuration)
            {
                this.connectionString = configuration.GetConnectionString("LoanDb");
            }
            
            public Task<LoanApplicationDto> Handle(Query request, CancellationToken cancellationToken)
            {
                using var cn = new NpgsqlConnection(connectionString);
            
                cn.Open();
                return Task.FromResult(cn
                    .Query<LoanApplicationDto, AddressDto, AddressDto, LoanApplicationDto>
                    (
                        BuildSelectDetailsQuery(),
                        (loan, customerAddress, propertyAddress) =>
                        {
                            loan.CustomerAddress = customerAddress;
                            loan.PropertyAddress = propertyAddress;
                            return loan;
                        },
                        new { ApplicationNumber = request.ApplicationNumber },
                        splitOn: "Country,Country")
                    .FirstOrDefault());
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

                query.AppendLine("FROM ddd_loan.loan_details_view ");
                query.AppendLine("WHERE number = :ApplicationNumber ");
                return query.ToString();
            }
        }
    }
}