using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M19G2.IBLL
{
    public interface IDeliveryAutomation
    {
        void AppendShipment(int orderId);
        void CompleteShipment(int orderId, int taxiId);
        ICollection<int> GetOrdersForStaff(int taxiId);

        void AddWorkforce(int taxiId);
    }
}
