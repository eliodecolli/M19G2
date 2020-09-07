using System;
using System.Collections.Generic;
using System.Linq;

namespace M19G2.Filters
{
    public class FilterDefinition<T>
    {
        public Func<T, bool> FunctionExpression { get; set; }

        public bool HasPassed { get; set; }

        public FilterDefinition(Func<T, bool> expression)
        {
            FunctionExpression = expression;
        }
    }

    public class Filter<T>
    {
        public ICollection<FilterDefinition<T>> FilterDefinitions { get; set; }

        private readonly ICollection<T> source;

        public Filter(ICollection<T> source)
        {
            FilterDefinitions = new List<FilterDefinition<T>>();
            this.source = source;
        }

        public ICollection<T> ApplyFilters()
        {
            if (FilterDefinitions.Count == 0)
            {
                return source;
            }

            var retvals = source.AsQueryable();

            foreach (var filter in FilterDefinitions)
            {
                retvals = retvals.Where(filter.FunctionExpression).AsQueryable();
                if (retvals.Count() > 0)
                    filter.HasPassed = true;
            }

            return retvals.ToList();
        }
    }
}