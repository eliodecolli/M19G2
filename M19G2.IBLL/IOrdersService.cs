using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using M19G2.Models;

namespace M19G2.IBLL
{
    public interface IOrdersService
    {
        int CreateNewOrder(OrderDto order);
        void CancelOrder(int orderId);
        void MarkOrderAsCompleted(int orderId, bool completed);

        ICollection<OrderDto> ListCompletedOrders();
        ICollection<OrderDto> ListOrdersForUser(int userId);
        ICollection<OrderDto> ListOrdersForAddress(int addressId);

        OrderDto GetOrderById(int orderId);
    }
}
