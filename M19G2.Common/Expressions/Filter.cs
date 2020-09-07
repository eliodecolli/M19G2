using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace M19G2.Common.Expressions
{
    public class Filter<T>
    {
        public Expression<Func<T, bool>> MegaFilter { get; set; }

        public void AddFilter(Expression<Func<T, bool>> expression)
        {
            if (MegaFilter == null)
            {
                MegaFilter = expression;
                return;
            }
            MegaFilter = MegaFilter.And(expression);
        }

        public void OrFilter(Expression<Func<T, bool>> expression)
        {
            if (MegaFilter == null)
            {
                MegaFilter = expression;
                return;
            }
            MegaFilter = MegaFilter.Or(expression);
        }
    }
}
