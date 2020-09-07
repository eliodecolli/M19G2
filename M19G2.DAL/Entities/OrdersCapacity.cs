using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M19G2.DAL.Entities
{
    public class OrderQuantity
    {
       public int OrderID { get; set; }

        public int DishID { get; set; }

        public int Quantity { get; set; }
    }
}
