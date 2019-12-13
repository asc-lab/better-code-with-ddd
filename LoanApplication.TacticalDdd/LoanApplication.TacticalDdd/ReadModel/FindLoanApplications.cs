using System.Collections.Generic;
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
    public static class FindLoanApplications
    {
        public class Query : IRequest<IEnumerable<LoanApplicationInfoDto>>
        {
            public LoanApplicationSearchCriteriaDto Criteria { get; set; }
        }
        
        public class Handler : IRequestHandler<Query, IEnumerable<LoanApplicationInfoDto>>
        {
            private readonly string connectionString;

            public Handler(IConfiguration configuration)
            {
                this.connectionString = configuration.GetConnectionString("LoanDb");
            }

            public async Task<IEnumerable<LoanApplicationInfoDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                await using var cn = new NpgsqlConnection(connectionString);
            
                cn.Open();
                return await cn
                           .QueryAsync<LoanApplicationInfoDto>(BuildSearchQuery(request.Criteria), request.Criteria);
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
                query.AppendLine("FROM ddd_loan.loan_details_view");
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
        }
    }
}