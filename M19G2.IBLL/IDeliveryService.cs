using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M19G2.IBLL
{
    public interface IDeliveryService
    {
        void SetEstimatedArivalTime(int orderId, int minutes);
        void MarkAsDelivered(int orderId, bool deliveredStatus);
    }
}
