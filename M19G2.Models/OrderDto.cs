using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M19G2.Models
{
    public class OrderDto
    {

        public OrderDto()
        {
            DishesIDs = new List<int>();
        }
        public int OrderID { get; set; }
        public int AddressID { get; set; }
        public int UserID { get; set; }
        public bool IsDelivered { get; set; }
        public int? ETA { get; set; }
        public string Message { get; set; }
        public List<int> DishesIDs { get; set; }

        public DateTime Created { get; set; }
        public bool Active { get; set; }

        public OrderDto Clone()
        {
            var retval = new OrderDto();
            int[] nIds = new int[DishesIDs.Count];

            DishesIDs.CopyTo(nIds);
            retval.DishesIDs = new List<int>();
            retval.DishesIDs.AddRange(nIds);

            retval.Active = Active;
            retval.AddressID = AddressID;
            retval.Created = Created;
            retval.ETA = ETA;
            retval.IsDelivered = IsDelivered;
            retval.UserID = UserID;
            retval.OrderID = OrderID;
            retval.Message = Message;

            return retval;
        }
    }
}
