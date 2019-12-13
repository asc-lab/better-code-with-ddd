using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DebtorRegistryMock.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DebtorInfoController : ControllerBase
    {
        private readonly ILogger<DebtorInfoController> logger;

        public DebtorInfoController(ILogger<DebtorInfoController> logger)
        {
            this.logger = logger;
        }

        [HttpGet("{pesel}")]
        public DebtorInfo Get([FromRoute]string pesel)
        {
            logger.Log(LogLevel.Information, $"Getting debtor info for pesel = {pesel}");
            
            if (pesel == "11111111116")
            {
                return new DebtorInfo
                {
                    Pesel = pesel,
                    Debts = new List<Debt>
                    {
                        new Debt { Amount = 3000M}
                    }
                };
            }
            else
            {
                return new DebtorInfo
                {
                    Pesel = pesel,
                    Debts = new List<Debt>()
                };
            }
        }
        
    }
}