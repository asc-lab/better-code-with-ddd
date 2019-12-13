using System.Collections.Generic;
using System.Threading.Tasks;
using LoanApplication.TacticalDdd.Application;
using LoanApplication.TacticalDdd.Application.Api;
using LoanApplication.TacticalDdd.ReadModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanApplication.TacticalDdd.PortsAdapters.WebApi
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LoanApplicationController : ControllerBase
    {
        private readonly IMediator bus;
        
        public LoanApplicationController(
            IMediator bus)
        {
            this.bus = bus;
        }
        
        [HttpPost]
        public async Task<string> Create([FromBody] LoanApplicationDto loanApplicationDto)
        {
            var newApplicationNumber = await bus.Send(new SubmitLoanApplication.Command
            {
                LoanApplication = loanApplicationDto,
                CurrentUser = User
            });
                
                
            return newApplicationNumber;
        }
        
        [HttpPost("evaluate/{applicationNumber}")]
        public async Task<IActionResult> Evaluate([FromRoute] string applicationNumber)
        {
            await bus.Send(new EvaluateLoanApplication.Command
            {
                ApplicationNumber = applicationNumber
            });
            return Ok();
        }
        
        [HttpPost("accept/{applicationNumber}")]
        public async Task<IActionResult> Accept([FromRoute] string applicationNumber)
        {
            await bus.Send(new AcceptLoanApplication.Command
            {
                ApplicationNumber = applicationNumber,
                CurrentUser = User
            });
            return Ok();
        }
        
        [HttpPost("reject/{applicationNumber}")]
        public async Task<IActionResult> Reject([FromRoute] string applicationNumber)
        {
            await bus.Send(new RejectLoanApplication.Command
            {
                ApplicationNumber = applicationNumber,
                CurrentUser = User
            });
            return Ok();
        }
        
        [HttpGet("{applicationNumber}")]
        public async Task<LoanApplicationDto> Get([FromRoute] string applicationNumber)
        {
            return await bus.Send(new GetLoanApplicationByNumber.Query {ApplicationNumber = applicationNumber});
        }
        
        [HttpPost("find")]
        public async Task<IEnumerable<LoanApplicationInfoDto>> Find([FromBody] LoanApplicationSearchCriteriaDto criteria)
        {
            return await bus.Send(new FindLoanApplications.Query {Criteria = criteria});
        }
    }
}