using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M19G2.Models;

namespace M19G2.IBLL
{
    public interface IOrderQueueService
    {
        void PushOrder(int orderId);
        int PopOrder(int staffId);
        List<int> GetOrdersInQueue(int staffId);
        int PeekCurrentOrder(int staffId);

        string OrderStatus(int orderId);

        void AddWorkforce(KitchenStaff staff);
    }
}
