using System.Security.Claims;
using LoanApplication.TacticalDdd.Application;
using LoanApplication.TacticalDdd.Application.Api;
using LoanApplication.TacticalDdd.ReadModel;
using O9d.AspNet.FluentValidation;
using static Microsoft.AspNetCore.Http.StatusCodes;
using Carter;

namespace LoanApplication.TacticalDdd.Web;


public class LoanApplicationApi : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("LoanApplication")
            .RequireAuthorization()
            .WithValidationFilter();
        
        group
            .MapPost("",([Validate] LoanApplicationSubmissionDto loanApplicationDto,ClaimsPrincipal user, LoanApplicationSubmissionService loanApplicationSubmissionService) =>
            {
                var newApplicationNumber = loanApplicationSubmissionService.SubmitLoanApplication(loanApplicationDto, user);
                return Results.Ok(newApplicationNumber);
            })
            .Produces<string>()
            .WithName("SubmitLoanApplication");
        
        group
            .MapPut("evaluate/{applicationNumber}", (string applicationNumber, LoanApplicationEvaluationService loanApplicationEvaluationService) =>
            {
                loanApplicationEvaluationService.EvaluateLoanApplication(applicationNumber);
                return Results.Ok();
            })
            .Produces(Status200OK);

        group
            .MapPut("accept/{applicationNumber}", async (string applicationNumber, ClaimsPrincipal user, LoanApplicationDecisionService loanApplicationDecisionService) =>
            {
                await loanApplicationDecisionService.AcceptApplication(applicationNumber,user);
                return Results.Ok();
            })
            .Produces(Status200OK);

        
        group
            .MapPut("reject/{applicationNumber}", async (string applicationNumber, ClaimsPrincipal user ,LoanApplicationDecisionService loanApplicationDecisionService) =>
            {
                await loanApplicationDecisionService.RejectApplication(applicationNumber,user, null);
                return Results.Ok();
            })
            .Produces(Status200OK);


        group
            .MapGet("{applicationNumber}", (string applicationNumber, LoanApplicationFinder loanApplicationFinder) => loanApplicationFinder.GetLoanApplication(applicationNumber))
            .Produces<LoanApplicationDto>();
        
        group
            .MapGet("find", (string applicationNumber, string customerNationalIdentifier,string decisionBy,string registeredBy, LoanApplicationFinder loanApplicationFinder) 
            => loanApplicationFinder.FindLoadApplication(new LoanApplicationSearchCriteriaDto(applicationNumber,customerNationalIdentifier,decisionBy,registeredBy)))
            .Produces<IList<LoanApplicationInfoDto>>();

    }
}