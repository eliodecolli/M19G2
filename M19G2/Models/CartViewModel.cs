using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using M19G2.Models;

namespace M19G2.Models
{
    public class CartViewModel
    {
        public OrderDto CurrentOrder { get; set; }

        public List<DishDto> Dishes { get; set; }

        public Dictionary<int, int> Quantities { get; set; }

        public bool CalledFromNav { get; set; }

        public bool Completed { get; set; }

        public List<SelectListItem> UserAddresses { get; set; }
    }
}