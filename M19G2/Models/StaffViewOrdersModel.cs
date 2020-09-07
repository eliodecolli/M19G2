using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M19G2.Models
{
    public class StaffViewOrdersModel
    {
        public List<OrderDto> Orders{get;set;}
        public int CurrentOrder{get;set;}
    }
}