using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M19G2.Models
{
    public class KitchenStaff
    {
        public int ID{get;set;}
        public string Name{get;set;}
        public int Capacity{get;set;}
        public Queue<int> Workload{get;set;}
    }
}
