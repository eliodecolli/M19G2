using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M19G2.Models
{
    public class OrderViewModel
    {
        public Dictionary<int, string[]> Orders { get; set; }

        public Dictionary<int, string> OrderDates { get; set; }

        public string PassingMessage { get; set; }  // which I should probably not use here
    }
}