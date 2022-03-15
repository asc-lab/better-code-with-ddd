using DebtorRegistryMock;

var builder = WebApplication.CreateBuilder();

var app = builder.Build();

app.MapGet("/DebtorInfo/{pesel}", (string pesel) =>
{
    app.Logger.LogInformation($"Getting debtor info for pesel = {pesel}");
    
    if (pesel == "11111111116")
    {
        return new DebtorInfo
        {
            Pesel = pesel,
            Debts = new ()
            {
                new Debt { Amount = 3000M}
            }
        };
    }
    
    return new DebtorInfo
    {
        Pesel = pesel,
        Debts = new ()
    };

});


app.Run();