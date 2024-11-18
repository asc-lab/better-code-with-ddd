using Microsoft.AspNetCore.Mvc;

namespace DebtorRegistryMock.Controllers;

[ApiController]
[Route("[controller]")]
public class DebtorInfoController(ILogger<DebtorInfoController> logger) : ControllerBase
{
    [HttpGet("{pesel}")]
    public DebtorInfo Get([FromRoute] string pesel)
    {
        logger.Log(LogLevel.Information, $"Getting debtor info for pesel = {pesel}");

        if (pesel == "11111111116")
        {
            return new DebtorInfo(pesel, [ new(3000M)]);
        }
        

        return new DebtorInfo(pesel,[]);
    }
}