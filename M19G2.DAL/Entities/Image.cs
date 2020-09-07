using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M19G2.DAL.Entities
{
    public class Image
    {
        public int ImageID { get; set; }

        public byte[] ImageData { get; set; }

        public virtual Dish Dish { get; set; }

        public int DishID { get; set; }
    }
}
