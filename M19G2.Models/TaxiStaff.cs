using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M19G2.Models
{
    public class TaxiStaff
    {
        public int ID { get; set; }
        public int WorkInProgress { get; set; }
        public List<int> Orders { get; set; }
    }
}
