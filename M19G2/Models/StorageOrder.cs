using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M19G2.Models
{
    public class StorageOrder
    {
        public Dictionary<int, int> Quantities {
            get {
                if (CurrentOrder == null)
                    return new Dictionary<int, int>();

                var retval = new Dictionary<int, int>();
                CurrentOrder.DishesIDs.ForEach(x =>
                {
                    if (retval.ContainsKey(x))
                        retval[x]++;
                    else
                        retval.Add(x, 1);
                });
                return retval;
            }
        }

        public OrderDto CurrentOrder { get; set; }

        public OrderDto CompileOrder()
        {
            var corder = CurrentOrder.Clone();
            corder.DishesIDs.Clear();

            foreach (var v in Quantities.Keys)
                for (int i = 0; i < Quantities[v]; i++)
                    corder.DishesIDs.Add(v);

            return corder;
        }
    }
}