using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M19G2.Models
{
    public class DeliveryViewModel
    {
        public int OrderID { get; set; }
        public string AddressName { get; set; }
        public int? ETA { get; set; }
    }
}