using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace M19G2.Common.Expressions
{
    public static class PredicateBuilder
    {
        public static Expression<T> Compose<T>(this Expression<T> p1, Expression<T> p2, Func<Expression, Expression, Expression> merger)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = p1.Parameters.Select((f, i) => new { f, s = p2.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = PredicateParameterRebinder.ReplaceParameters(map, p2.Body);

            // apply composition of lambda expression bodies to parameters from the first expression
            return Expression.Lambda<T>(merger(p1.Body, secondBody), p1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> p1, Expression<Func<T, bool>> p2)
        {
            return p1.Compose(p2, Expression.And);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> p1, Expression<Func<T, bool>> p2)
        {
            return p1.Compose(p2, Expression.Or);
        }
    }
}
