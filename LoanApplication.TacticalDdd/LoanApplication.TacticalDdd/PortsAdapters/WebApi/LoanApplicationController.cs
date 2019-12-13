using System.Collections.Generic;
using LoanApplication.TacticalDdd.Application;
using LoanApplication.TacticalDdd.Application.Api;
using LoanApplication.TacticalDdd.ReadModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanApplication.TacticalDdd.PortsAdapters.WebApi
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LoanApplicationController : ControllerBase
    {
        private readonly LoanApplicationSubmissionService loanApplicationSubmissionService;
        private readonly LoanApplicationEvaluationService loanApplicationEvaluationService;
        private readonly LoanApplicationDecisionService loanApplicationDecisionService;
        private readonly LoanApplicationFinder loanApplicationFinder;

        public LoanApplicationController(
            LoanApplicationSubmissionService loanApplicationSubmissionService,
            LoanApplicationEvaluationService loanApplicationEvaluationService,
            LoanApplicationFinder loanApplicationFinder, 
            LoanApplicationDecisionService loanApplicationDecisionService)
        {
            this.loanApplicationSubmissionService = loanApplicationSubmissionService;
            this.loanApplicationEvaluationService = loanApplicationEvaluationService;
            this.loanApplicationFinder = loanApplicationFinder;
            this.loanApplicationDecisionService = loanApplicationDecisionService;
        }
        
        [HttpPost]
        public string Create([FromBody] LoanApplicationDto loanApplicationDto)
        {
            var newApplicationNumber = loanApplicationSubmissionService.SubmitLoanApplication(loanApplicationDto, User);
            return newApplicationNumber;
        }
        
        [HttpPost("evaluate/{applicationNumber}")]
        public IActionResult Evaluate([FromRoute] string applicationNumber)
        {
            loanApplicationEvaluationService.EvaluateLoanApplication(applicationNumber);
            return Ok();
        }
        
        [HttpPost("accept/{applicationNumber}")]
        public IActionResult Accept([FromRoute] string applicationNumber)
        {
            loanApplicationDecisionService.AcceptApplication(applicationNumber,User);
            return Ok();
        }
        
        [HttpPost("reject/{applicationNumber}")]
        public IActionResult Reject([FromRoute] string applicationNumber)
        {
            loanApplicationDecisionService.RejectApplication(applicationNumber,User, null);
            return Ok();
        }
        
        [HttpGet("{applicationNumber}")]
        public LoanApplicationDto Get([FromRoute] string applicationNumber)
        {
            return loanApplicationFinder.GetLoanApplication(applicationNumber);
        }
        
        [HttpPost("find")]
        public IList<LoanApplicationInfoDto> Find([FromBody] LoanApplicationSearchCriteriaDto criteria)
        {
            return loanApplicationFinder.FindLoadApplication(criteria);
        }
    }
}