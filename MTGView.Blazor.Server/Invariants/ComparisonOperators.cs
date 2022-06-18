using System.Linq.Expressions;

namespace MTGView.Blazor.Server.Invariants;
public sealed class ComparisonOperators
{
    public static readonly Dictionary<String, Expression<Func<Decimal, Decimal, bool>>> OperatorMap = new ()
    {
        {"<",(a,b) => a < b},
        {">",(a,b) => a > b},
        {"<=",(a,b) => a <= b},
        {">=",(a,b) => a >= b},
        {"=",(a,b) => a == b},
    };
}
