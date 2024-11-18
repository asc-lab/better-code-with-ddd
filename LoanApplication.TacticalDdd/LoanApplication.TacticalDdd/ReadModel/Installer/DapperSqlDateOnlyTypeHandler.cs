using System.Data;
using Dapper;

namespace LoanApplication.TacticalDdd.ReadModel.Installer;

public class DapperSqlDateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override void SetValue(IDbDataParameter parameter, DateOnly date)
        => parameter.Value = date.ToDateTime(new TimeOnly(0, 0));
    
    public override DateOnly Parse(object value)
        => DateOnly.FromDateTime((DateTime)value);
}