using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using LoanApplication.BusinessLogic;
using LoanApplication.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanApplication.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LoanApplicationController : ControllerBase
    {
        private readonly LoanApplicationService loanApplicationService;

        public LoanApplicationController(LoanApplicationService loanApplicationService)
        {
            this.loanApplicationService = loanApplicationService;
        }
        
        [HttpPost]
        public async Task<LoanApplicationDto> Create([FromBody] LoanApplicationDto loanApplicationDto)
        {
            var newApplicationNumber = await loanApplicationService.CreateLoanApplication(loanApplicationDto, User);
            return await loanApplicationService.GetLoanApplication(newApplicationNumber);
        }
        
        [HttpPost("find")]
        public async Task<IList<LoanApplicationInfoDto>> Find([FromBody] LoanApplicationSearchCriteriaDto criteria)
        {
            return await loanApplicationService.FindLoanApplication(criteria);
        }
        
        [HttpPost("evaluate/{applicationNumber}")]
        public async Task<LoanApplicationDto> Evaluate([FromRoute] string applicationNumber)
        {
            await loanApplicationService.EvaluateLoanApplication(applicationNumber);
            return await loanApplicationService.GetLoanApplication(applicationNumber);
        }
        
        [HttpPost("accept/{applicationNumber}")]
        public async Task<LoanApplicationDto> Accept([FromRoute] string applicationNumber)
        {
            await loanApplicationService.AcceptApplication(applicationNumber,User);
            return await loanApplicationService.GetLoanApplication(applicationNumber);
        }
        
        [HttpPost("reject/{applicationNumber}")]
        public async Task<LoanApplicationDto> Reject([FromRoute] string applicationNumber)
        {
            await loanApplicationService.RejectApplication(applicationNumber,User, null);
            return await loanApplicationService.GetLoanApplication(applicationNumber);
        }
        
        [HttpGet("{applicationNumber}")]
        public async Task<LoanApplicationDto> Get([FromRoute] string applicationNumber)
        {
            return await loanApplicationService.GetLoanApplication(applicationNumber);
        }
        
        
    }
}