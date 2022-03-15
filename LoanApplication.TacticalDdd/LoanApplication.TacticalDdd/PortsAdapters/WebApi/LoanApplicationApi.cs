using System.Security.Claims;
using LoanApplication.TacticalDdd.Application;
using LoanApplication.TacticalDdd.Application.Api;
using LoanApplication.TacticalDdd.ReadModel;

namespace LoanApplication.TacticalDdd.PortsAdapters.WebApi;

using Carter;

public class LoanApplicationApi : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app
            .MapPost("LoanApplication",(LoanApplicationSubmissionDto loanApplicationDto,ClaimsPrincipal user, LoanApplicationSubmissionService loanApplicationSubmissionService) =>
            {
                var newApplicationNumber = loanApplicationSubmissionService.SubmitLoanApplication(loanApplicationDto, user);
                return Results.Ok(newApplicationNumber);
            })
            .RequireAuthorization()
            .Produces<string>()
            .WithName("SubmitLoanApplication");
        
        app
            .MapPut("LoanApplication/evaluate/{applicationNumber}", (string applicationNumber, LoanApplicationEvaluationService loanApplicationEvaluationService) =>
            {
                loanApplicationEvaluationService.EvaluateLoanApplication(applicationNumber);
                return Results.Ok();
            })
            .RequireAuthorization()
            .Produces(200);

        app
            .MapPut("LoanApplication/accept/{applicationNumber}", (string applicationNumber, ClaimsPrincipal user, LoanApplicationDecisionService loanApplicationDecisionService) =>
            {
                loanApplicationDecisionService.AcceptApplication(applicationNumber,user);
                return Results.Ok();
            })
            .RequireAuthorization()
            .Produces(200);

        
        app
            .MapPut("LoanApplication/reject/{applicationNumber}", (string applicationNumber, ClaimsPrincipal user ,LoanApplicationDecisionService loanApplicationDecisionService) =>
            {
                loanApplicationDecisionService.RejectApplication(applicationNumber,user, null);
                return Results.Ok();
            })
            .RequireAuthorization()
            .Produces(200);


        app
            .MapGet("LoanApplication/{applicationNumber}", (string applicationNumber, LoanApplicationFinder loanApplicationFinder) => loanApplicationFinder.GetLoanApplication(applicationNumber))
            .RequireAuthorization()
            .Produces<LoanApplicationDto>();
        
        app
            .MapGet("LoanApplication/find", (string applicationNumber, string customerNationalIdentifier,string decisionBy,string registeredBy, LoanApplicationFinder loanApplicationFinder) 
            => loanApplicationFinder.FindLoadApplication(new LoanApplicationSearchCriteriaDto(applicationNumber,customerNationalIdentifier,decisionBy,registeredBy)))
            .RequireAuthorization()
            .Produces<IList<LoanApplicationInfoDto>>();

    }
}