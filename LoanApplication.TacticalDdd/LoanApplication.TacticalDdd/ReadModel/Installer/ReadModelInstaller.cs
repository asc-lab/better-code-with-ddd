using Dapper;

namespace LoanApplication.TacticalDdd.ReadModel.Installer;

public static class ReadModelInstaller
{
    public static void AddReadModelServices(this IServiceCollection services, string connectionString)
    {
        SqlMapper.AddTypeHandler(new DapperSqlDateOnlyTypeHandler());
    }
}